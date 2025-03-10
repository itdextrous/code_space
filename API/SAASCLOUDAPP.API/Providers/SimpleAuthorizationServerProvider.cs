using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using SAASCLOUDAPP.BusinessLayer;
using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.DataAccessLayer;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using Workfacta.Common.Enums;
using Workfacta.Entities;
using Workfacta.Logic.Helpers;
using Workfacta.Logic.Services;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.API.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly AuthRepository authRepository;
        private readonly IMongoContext mongoContext;
        private readonly RoleManager roleManager;
        private readonly CompanyRepository companyRepository;
        private readonly TeamRepository teamRepository;
        private readonly PaymentRepository paymentRepository;
        private readonly DaysRepository daysRepository;
        private readonly IDateProvider _dateProvider;
        private readonly GlobalConfigurationService _globalConfigurationService;

        public SimpleAuthorizationServerProvider(AuthRepository authRepository, IMongoContext mongoContext, RoleManager roleManager, CompanyRepository companyRepository,
            TeamRepository teamRepository, PaymentRepository paymentRepository, DaysRepository daysRepository, IDateProvider dateProvider, GlobalConfigurationService globalConfigurationService)
        {
            this.authRepository = authRepository;
            this.mongoContext = mongoContext;
            this.roleManager = roleManager;
            this.companyRepository = companyRepository;
            this.teamRepository = teamRepository;
            this.paymentRepository = paymentRepository;
            this.daysRepository = daysRepository;
            _dateProvider = dateProvider;
            _globalConfigurationService = globalConfigurationService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = default(string);
            var clientSecret = default(string);
            var client = default(Client);
            var role = context.Parameters["role"];
            context.OwinContext.Set<string>("as:role", role);
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }

            client = authRepository.FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));

                return Task.FromResult<object>(null);

            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");

                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");

                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var roleId = context.OwinContext.Get<string>("as:role");
                var sanitisedUsername = context.UserName.Replace("%2b", "+").Replace("%26", "&").ToLowerInvariant();

                var user = !string.IsNullOrEmpty(roleId) && roleId != "undefined"
                    ? await authRepository.FindUserWithRole(sanitisedUsername, context.Password, roleId)
                    : await authRepository.FindUser(sanitisedUsername, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var registrationData = mongoContext.Registration.AsQueryable().FirstOrDefault(r => r.userId == user.Id && r.isActive && r.isDeleted == false);
                if (registrationData == null)
                {
                    context.SetError("user_inactive", "The user has been deactivated.");
                    return;
                }

                var permissionQuery = Query<Permission>.EQ(z => z.userId, user.Id);
                var permissionData = mongoContext.Permission.Find(permissionQuery).SetLimit(1).FirstOrDefault();

                var role = string.IsNullOrEmpty(registrationData.RoleId) ? null : await roleManager.FindByIdAsync(registrationData.RoleId);
                if (role == null || !Enum.TryParse(role.Name, out RolesEnum roleEnum))
                {
                    context.SetError("user_inactive", "The user has been deactivated.");
                    return;
                }

                var companyObj = string.IsNullOrEmpty(registrationData.CompanyId) ? null : await companyRepository.getCompanyById(registrationData.CompanyId);
                if (companyObj == null)
                {
                    context.SetError("company_inactive", "The company has been deactivated");
                    return;
                }

                var clientInstance = mongoContext.ClientInstance.FindOneById(companyObj.clientInstanceId);
                if (clientInstance == null)
                {
                    context.SetError("company_inactive", "The company has been deactivated");
                    return;
                }

                if (clientInstance.OnboardingStatus != OnboardingStatus.Complete && !new[] { RolesEnum.SuperAdmin, RolesEnum.EnterpriseAdmin }.Contains(roleEnum))
                {
                    context.SetError("company_inactive", "The company has not yet been set up");
                    return;
                }

                var companyId = companyObj.Id;
                var companyName = companyObj.CompanyName;
                var enterprisePackEnabled = companyObj.enterprisePackEnabled;

                if (string.IsNullOrEmpty(registrationData.TeamId) && registrationData.teamsArray.Length > 0)
                {
                    registrationData.TeamId = registrationData.teamsArray[0];
                }

                var teamId = string.Empty;
                var teamName = string.Empty;

                var teamsObj = string.IsNullOrEmpty(registrationData.TeamId) ? null : await teamRepository.getTeamById(registrationData.TeamId);
                if (teamsObj != null)
                {
                    teamId = teamsObj.Id;
                    teamName = teamsObj.TeamName;
                }

                var planIsActive = false;
                var isRenew = false;
                var isCompanyFree = false;
                var isCompayAccess = false;
                var isTeamAccess = false;
                if (!string.IsNullOrEmpty(registrationData.CompanyId) && companyObj != null && teamsObj != null)
                {
                    isCompayAccess = companyObj.isAccess;
                    isCompanyFree = companyObj.isFree || companyObj.PaymentType == PaymentType.Invoiced;
                    isTeamAccess = teamsObj.isAccess;

                    if (roleEnum == RolesEnum.SuperAdmin || (isCompanyFree && isTeamAccess))
                    {
                        planIsActive = true;
                        isRenew = true;
                    }
                    else if (!isCompanyFree && isCompayAccess)
                    {
                        var filterS = Query.EQ("companyId", registrationData.CompanyId);
                        var companySubscription = mongoContext.Subscription.Find(filterS).FirstOrDefault();

                        if (companySubscription != null)
                        {
                            isRenew = true;
                            if (companySubscription.IsCurrent(_dateProvider.UtcNow))
                            {
                                planIsActive = true;
                            }
                        }
                    }
                }
                if (companyObj.isAccess && teamsObj == null)
                {
                    planIsActive = true;
                }

                var getPermissionData = await paymentRepository.GetPermissionByPlan(new permissionsPlan
                {
                    userId = user.Id,
                    companyId = registrationData.CompanyId,
                    userEmail = user.Email
                });

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim("sub", user.UserName));
                identity.AddClaim(new Claim("role", role.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                var dateFormat = daysRepository.GetCompanyDateFormat(companyId);
                //Check if user's client instance related to UniSa-AUBCG
                var isUniSaType = await companyRepository.IsCompanyRelatedToUNISA(companyId);

                var isEnterpriseMenu = false;
                if (role.Name == RolesEnum.SuperAdmin.ToString() || (role.Name == RolesEnum.EnterpriseAdmin.ToString() && clientInstance.IsEnterprise && enterprisePackEnabled))
                {
                    isEnterpriseMenu = true;
                }
                var propsDict = new Dictionary<string, string>
                {
                    {
                        "as:client_id", context.ClientId ?? string.Empty
                    },
                    {
                        "userName", user.UserName
                    },
                    {
                        "EmailConfirmed", user.EmailConfirmed.ToString()
                    },
                    {
                        "IsActive", registrationData.isActive.ToString()
                    },
                    {
                        "PermissionId", permissionData == null ? string.Empty : permissionData.id
                    },
                    {
                        "RoleId", registrationData.RoleId
                    },
                    {
                        "UserId", user.Id
                    },
                    {
                        "RoleName", role.Name ?? string.Empty
                    },
                    {
                        "CompanyId", companyId
                    },
                    {
                        "CompanyName", companyName
                    },
                    {
                        "TeamId", teamId
                    },
                    {
                        "TeamName", teamName
                    },
                    {
                        "DefaultLanguage", registrationData.defaultLanguage ?? string.Empty
                    },
                    {
                        "planIsActive", planIsActive.ToString()
                    },
                    {
                        "isRenew", isRenew.ToString()
                    },
                    {
                        "isCompanyFree", isCompanyFree.ToString()
                    },
                    {
                        "clientInstanceId", clientInstance.Id
                    },
                    {
                        "getPermissionPalnData", getPermissionData == null ? null : JsonConvert.SerializeObject(new { getPermissionData = getPermissionData })
                    },
                    {
                        "dateFormatPreference", dateFormat
                    },
                    {
                        "permissions", JsonConvert.SerializeObject(new { permissionData = permissionData?.permittedPagesArray ?? new string[] { } })
                    },
                    {
                        "isUniSaType", isUniSaType.ToString()
                    },
                    {
                        "enterprisePackEnabled", enterprisePackEnabled.ToString()
                    },
                    {
                        "isEnterpriseMenu", isEnterpriseMenu.ToString()
                    }
                };

                if (companyObj?.CompanyLogoUrl != null)
                {
                    propsDict["CompanyLogoUrl"] = companyObj.CompanyLogoUrl;
                }

                if (registrationData.ProfilePictureUrl != null)
                {
                    propsDict["ProfilePictureUrl"] = registrationData.ProfilePictureUrl;
                }
                else
                {
                    // BETA-1605 - The iOS app is crashing without a profile picture.
                    // TODO: Remove this case once this has been fixed on the app
                    propsDict["ProfilePictureUrl"] = "https://assets.workfacta.com/emails/2021-06/default-profile-picture.png";
                }

                if (AppSettings.EnableTimeMachine)
                {
                    var timeMachineConfig = await _globalConfigurationService.GetTimeMachineConfigAsync();
                    var initialDate = timeMachineConfig?.InitialDate;
                    if (initialDate != null)
                    {
                        propsDict["TimeMachineInitialDate"] = initialDate.Value.ToString("O");
                    }
                }

                var props = new AuthenticationProperties(propsDict);

                //////////////////////////User Audit 
                mongoContext.UserAudit.Insert(new UserAudit
                {
                    id = Guid.NewGuid().ToString(),
                    createdDate = _dateProvider.UtcNow,
                    userId = user.Id,
                    companyId = companyId,
                    teamId = teamId,
                    eventName = "Log In",
                    eventType = "User Login",
                    userName = registrationData.FirstName + " " + registrationData.LastName,
                    companyName = companyName,
                    teamName = teamName,
                    isDeleted = false,
                    iPAddress = Helper.GetIPAddress()
                });
                /////////////////////////

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");

                return Task.FromResult<object>(null);
            }
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);

            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}
