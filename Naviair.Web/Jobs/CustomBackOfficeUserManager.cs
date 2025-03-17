using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Net;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Security;

namespace Naviair.Web.Jobs
{
    public class CustomBackOfficeUserManager : BackOfficeUserManager
    {
        public CustomBackOfficeUserManager(
            IIpResolver ipResolver, 
            IUserStore<BackOfficeIdentityUser> store, 
            IOptions<BackOfficeIdentityOptions> optionsAccessor, 
            IPasswordHasher<BackOfficeIdentityUser> passwordHasher, 
            IEnumerable<IUserValidator<BackOfficeIdentityUser>> userValidators, 
            IEnumerable<IPasswordValidator<BackOfficeIdentityUser>> passwordValidators, 
            BackOfficeErrorDescriber errors, IServiceProvider services, 
            IHttpContextAccessor httpContextAccessor, 
            ILogger<UserManager<BackOfficeIdentityUser>> logger, 
            IOptions<UserPasswordConfigurationSettings> passwordConfiguration, 
            IEventAggregator eventAggregator, 
            IBackOfficeUserPasswordChecker backOfficeUserPasswordChecker
            ) : base(
                ipResolver, 
                store, 
                optionsAccessor, 
                passwordHasher, 
                userValidators, 
                passwordValidators, 
                errors, 
                services, 
                httpContextAccessor, 
                logger, 
                passwordConfiguration, 
                eventAggregator, 
                backOfficeUserPasswordChecker
                )
        {
        }
        
    }
}
