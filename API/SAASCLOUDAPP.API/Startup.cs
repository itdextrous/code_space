using System;
using System.Configuration;
using System.Web.Http;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.WebApi;
using AutoMapper;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.NLogTarget;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using NLog;
using NLog.Config;
using Owin;
using SAASCLOUDAPP.API.Authorization;
using SAASCLOUDAPP.API.Providers;
using SAASCLOUDAPP.API.Services;
using SAASCLOUDAPP.BusinessLayer;
using SAASCLOUDAPP.BusinessLayer.Services;
using SAASCLOUDAPP.BusinessLayer.Services.Interfaces;
using SAASCLOUDAPP.DataAccessLayer;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using Workfacta.Data;
using Workfacta.Data.Repositories;
using Workfacta.Data.Repositories.Interfaces;
using Workfacta.Library;
using Workfacta.Logic.Configuration;
using Workfacta.Logic.Services;
using Workfacta.Logic.Services.Queues;
using Workfacta.Logic.Services.Storage;
using Workfacta.Models;
using Workfacta.Models.Builders;
using Workfacta.Models.Integrations.Xero;
using Workfacta.Shared.Providers;
using Workfacta.Shared.Services;
using Workfacta.Shared.Services.Storage;
using Workfacta.Templating.Services;

[assembly: OwinStartup(typeof(SAASCLOUDAPP.API.Startup))]
namespace SAASCLOUDAPP.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new MongoContext(AppSettings.MongoDbContext)).AsImplementedInterfaces().SingleInstance();
            builder.Register(c => new WorkfactaContext(AppSettings.MongoDbContext)).As<IWorkfactaContext>().SingleInstance();
            builder.RegisterType<MongoContextInitialiser>();

            builder.Register(c => AutoMapperConfig.Build()).As<IMapper>().SingleInstance();
            builder.Register(c => c.Resolve<GlobalConfigurationService>().GetEmailConfig()).SingleInstance();
            builder.Register(c => new EmailConfiguration
            {
                SmtpServer = AppSettings.SmtpServer,
                SmtpPort = AppSettings.SmtpPort,
                SmtpUsername = AppSettings.SmtpUsername,
                SmtpPassword = AppSettings.SmtpPassword,
                SmtpEnableSsl = AppSettings.SmtpEnableSsl,
                EmailSender = AppSettings.EmailSender,

                IsTestMode = AppSettings.IsTestServer,
                TestEmailReceiver = AppSettings.WorkfactaDemoEmail,
                SubjectPrefix = AppSettings.SubjectPrefix,
                TestCompanyIds = AppSettings.TestCompanyIds,

                BlacklistedDomains = c.ResolveOptional<EmailConfig>()?.BlacklistedDomains
            }).SingleInstance();
            builder.Register(c => new ContentBuilderConfiguration
            {
                SupportEmail = AppSettings.WorkfactaSupportEmail,
                BaseWebUrl = AppSettings.BaseWebUrl
            }).SingleInstance();
            builder.RegisterType<EmailService>().As<IEmailService>();
            builder.RegisterType<EventLogger>().As<IEventLogger>();

            builder.RegisterType<ContentBuilder>();

            if (AppSettings.EnableTimeMachine)
            {
                builder.RegisterType<DateProviderWithHeaderOverride>().As<IDateProvider>();
                builder.RegisterType<DateProviderWithHeaderOverrideFactory>().As<IDateProviderFactory>();
            }
            else
            {
                builder.RegisterType<DefaultDateProvider>().As<IDateProvider>();
                builder.RegisterType<DefaultDateProviderFactory>().As<IDateProviderFactory>();
            }

            if (AppSettings.IsDemoServer)
            {
                builder.RegisterType<CloneService>();
            }

            builder.RegisterType<ClientIpAddressAccessor>().As<IClientIpAddressAccessor>();
            builder.RegisterType<UserAccessor>().As<IUserAccessor>().InstancePerRequest();
            builder.RegisterType<AuthorizationService>().InstancePerRequest();


            builder.RegisterType<AzureFileNameGenerator>().As<IFileNameGenerator>().SingleInstance();
            builder.Register(c => new AttachmentStorageClient(AppSettings.FileStorageConnectionString, AppSettings.AttachmentContainerName)).SingleInstance();
            builder.Register(c => new ImageStorageClient(AppSettings.FileStorageConnectionString, AppSettings.ImageContainerName)).SingleInstance();
            builder.Register(c => new ReportStorageClient(AppSettings.FileStorageConnectionString, AppSettings.ReportContainerName)).SingleInstance();
            builder.RegisterType<AttachmentStorageService>().As<IAttachmentStorageService>();
            builder.RegisterType<ImageStorageService>().As<IImageStorageService>();
            builder.RegisterType<ReportStorageService>().As<IReportStorageService>();


            builder.Register(c => new UpdateSubscriptionQueueClient(AppSettings.QueueStorageConnectionString, AppSettings.UpdateSubscriptionQueueName)).SingleInstance();
            builder.RegisterType<UpdateSubscriptionQueueService>().As<IUpdateSubscriptionQueueService>();

            builder.Register(c => new ActionNotificationQueueClient(AppSettings.QueueStorageConnectionString, AppSettings.ActionNotificationQueueName)).SingleInstance();
            builder.RegisterType<ActionNotificationQueueService>().As<IActionNotificationQueueService>();

            builder.Register(c => new PdfGenerator(c.Resolve<IReportStorageService>(), c.Resolve<IFileNameGenerator>())).As<IPdfGenerator>();

            builder.RegisterType<AuthRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.TeamRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.CompanyRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.DepartmentRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.MeetingRepository>().SingleInstance();
            builder.RegisterType<MeetingReminderRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.PermissionRepository>().SingleInstance();
            builder.RegisterType<PageService>();
            builder.RegisterType<PlanRepository>().SingleInstance();
            builder.RegisterType<VisionTypeRepository>().SingleInstance();
            builder.RegisterType<StrategyTypeRepository>().SingleInstance();
            builder.RegisterType<StatusRepository>().SingleInstance();
            builder.RegisterType<MarketStrategyRepository>().SingleInstance();
            builder.RegisterType<VisionRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.NumbersRepository>().SingleInstance();
            builder.RegisterType<CaptureMethodRepository>().SingleInstance();
            builder.RegisterType<GoalsRepository>().SingleInstance();
            builder.RegisterType<GoalStatusRepository>().SingleInstance();
            builder.RegisterType<CommonStatusRepository>().SingleInstance();
            builder.RegisterType<NumberStatusRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.ActionRepository>().SingleInstance();
            builder.RegisterType<IssuesRepository>().SingleInstance();
            builder.RegisterType<DaysRepository>().SingleInstance();
            builder.RegisterType<FrequencyRepository>().SingleInstance();
            builder.RegisterType<SuggestionRepository>().SingleInstance();
            builder.RegisterType<WorkCategoryRepository>().SingleInstance();
            builder.RegisterType<LanguageRepository>().SingleInstance();
            builder.RegisterType<ScoreRepository>().SingleInstance();
            builder.RegisterType<PartnerRepository>().SingleInstance();
            builder.RegisterType<TokenRepository>().SingleInstance();
            builder.RegisterType<IssueTypeRepository>().SingleInstance();
            builder.RegisterType<FirebaseRepository>().SingleInstance();
            builder.RegisterType<FinancialRepository>().SingleInstance();
            builder.RegisterType<PageVideoRepository>().SingleInstance();
            builder.RegisterType<GraphDataRepository>().SingleInstance();
            builder.RegisterType<NotesRepository>().SingleInstance();
            builder.RegisterType<PaymentRepository>().SingleInstance();
            builder.RegisterType<BusinessLayer.EventHistoryRepository>().SingleInstance();
            builder.RegisterType<TacticsRepository>().SingleInstance();
            builder.RegisterType<DashboardRepository>().SingleInstance();
            builder.Register(context => new Stripe.StripeClient(AppSettings.StripeApiKey)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StripeService>().SingleInstance();
            builder.RegisterType<ManageUserService>().InstancePerRequest();
            builder.RegisterType<ManageTeamService>().InstancePerRequest();
            builder.RegisterType<ConfigTemplateService>();
            builder.RegisterType<PlanningPeriodService>().As<IPlanningPeriodService>();
            builder.RegisterType<MeetingScheduleService>();
            builder.RegisterType<UpdateScheduleService>().As<IUpdateScheduleService>();
            builder.RegisterType<ManagePartnerService>().InstancePerRequest();
            builder.RegisterType<ManageClientService>().InstancePerRequest();
            builder.RegisterType<RegistrationService>().InstancePerRequest();
            builder.RegisterType<HelpService>().InstancePerRequest();
            builder.RegisterType<UserService>();
            builder.RegisterType<GlobalConfigurationService>();
            builder.RegisterType<GlobalFeatureFlagService>();
            builder.RegisterType<FeatureFlagService>();
            builder.RegisterType<FeatureFlagManagementService>();
            builder.RegisterType<PermissionService>();
            builder.RegisterType<ManagePaymentService>();
            builder.RegisterType<PeriodService>();
            builder.RegisterType<EmailGenerationService>();
            builder.RegisterType<ChartingService>();
            builder.RegisterType<PlanningPeriodGenerator>();
            builder.RegisterType<UpdateScheduleGenerator>();

            var xeroConfiguration = new XeroConfiguration(AppSettings.BaseWebUrl)
            {
                ClientId = AppSettings.XeroClientId,
                ClientSecret = AppSettings.XeroClientSecret,
            };
            builder.RegisterInstance<XeroConfigurationClientInfo>(xeroConfiguration);
            builder.RegisterInstance(xeroConfiguration);
            builder.RegisterType<XeroAuthManagerBuilder>();
            builder.RegisterType<XeroDataSourceBuilder>().As<IXeroDataSourceBuilder>();
            builder.RegisterType<XeroConnection>();

            builder.RegisterType<ApplicationUserManager>();
            builder.RegisterType<RoleManager>();
            builder.RegisterType<MeetingService>().As<IMeetingService>().SingleInstance();
            builder.RegisterType<PublicHolidaysRepository>().As<IPublicHolidaysRepository>().SingleInstance();
            builder.RegisterType<PublicHolidaysService>().As<IPublicHolidaysService>().SingleInstance();
            builder.RegisterType<PlanningPeriodGroupRepository>().As<IPlanningPeriodGroupRepository>();
            builder.RegisterType<PlanningPeriodRepository>().As<IPlanningPeriodRepository>();
            builder.RegisterType<MeetingScheduleRepository>().As<IMeetingScheduleRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<PartnerDetailsRepository>().As<IPartnerDetailsRepository>();
            builder.RegisterType<PartnerUserDetailsRepository>().As<IPartnerUserDetailsRepository>();
            builder.RegisterType<GlobalConfigurationRepository>().As<IGlobalConfigurationRepository>();
            builder.RegisterType<CloneTemplateRepository>().As<ICloneTemplateRepository>();
            builder.RegisterType<FeatureFlagRepository>().As<IFeatureFlagRepository>();
            builder.RegisterType<FeatureFlagInstanceRepository>().As<IFeatureFlagInstanceRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.CompanyRepository>().As<ICompanyRepository>();
            builder.RegisterType<CompanyExternalDataSourceConfigRepository>().As<ICompanyExternalDataSourceConfigRepository>();
            builder.RegisterType<ExternalDataConfigRepository>().As<IExternalDataConfigRepository>();
            builder.RegisterType<ClientInstanceRepository>().As<IClientInstanceRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.TeamRepository>().As<ITeamRepository>();
            builder.RegisterType<PageRepository>().As<IPageRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.PermissionRepository>().As<IPermissionRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.DepartmentRepository>().As<IDepartmentRepository>();
            builder.RegisterType<RegistrationRepository>().As<IRegistrationRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<SubscriptionRepository>().As<ISubscriptionRepository>();
            builder.RegisterType<StripeProductRepository>().As<IStripeProductRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.EventHistoryRepository>().As<IEventHistoryRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.NumberRepository>().As<INumberRepository>();
            builder.RegisterType<NumberRecordRepository>().As<INumberRecordRepository>();
            builder.RegisterType<Workfacta.Data.Repositories.ActionRepository>().As<IActionRepository>();
            builder.RegisterType<IssueRepository>().As<IIssueRepository>();
            builder.RegisterType<GoalRepository>().As<IGoalRepository>();

            builder.RegisterType<LinksRepository>();

            builder.RegisterType<NewsRepository>().As<INewsRepository>().SingleInstance();
            builder.RegisterType<NewsService>().As<INewsService>().SingleInstance();

            builder.RegisterType<RecurringActionRepository>().As<IRecurringActionRepository>().SingleInstance();
            builder.RegisterType<RecurringActionService>().As<IRecurringActionService>().SingleInstance();

            builder.RegisterType<ReportEmailingService>().As<IReportEmailingService>().SingleInstance();

            builder.RegisterType<UserStore>().As<IUserStore<User>>();
            builder.RegisterType<RoleStore>().As<IRoleStore<Role>>();
            builder.Register(context => app.GetDataProtectionProvider()).As<IDataProtectionProvider>();
            builder.RegisterType<SimpleAuthorizationServerProvider>()
                .AsImplementedInterfaces<IOAuthAuthorizationServerProvider, ConcreteReflectionActivatorData>().SingleInstance();

            builder.RegisterType<AttachmentStorage>().As<IAttachmentStorage>();

            builder.RegisterType<WorkfactaModel>();
            builder.RegisterType<WorkfactaModelRepositories>();
            builder.RegisterType<WorkfactaModelServices>();
            builder.RegisterType<NumberBuilder>();
            builder.RegisterType<NumberRecordBuilder>();
            builder.RegisterType<QuarterlyNumbersBuilder>();
            builder.RegisterType<LinkedItemsBuilder>();
            builder.RegisterType<ActionBuilder>();
            builder.RegisterType<QuarterlyGoalsBuilder>();
            builder.RegisterType<GoalBuilder>();

            builder.RegisterType<SimpleRefreshTokenProvider>()
                .AsImplementedInterfaces<IAuthenticationTokenProvider, ConcreteReflectionActivatorData>().SingleInstance();

            builder.RegisterApiControllers(typeof(Startup).Assembly);

            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            var webApiDependencyResolver = new AutofacWebApiDependencyResolver(container);

            var configuration = new HttpConfiguration
            {
                DependencyResolver = webApiDependencyResolver
            };

            var appInsightsConnStr = ConfigurationManager.AppSettings["APPLICATIONINSIGHTS_CONNECTION_STRING"];
            if (!string.IsNullOrEmpty(appInsightsConnStr))
            {
                TelemetryConfiguration.Active.ConnectionString = appInsightsConnStr;
            }
            ConfigureNlog(ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            ConfigureOAuth(app, container);

            WebApiConfig.Register(configuration);

            app.UseWebApi(configuration);

            app.UseAutofacWebApi(configuration);

            InitialiseAzureStorage(container);
            InitialiseQueues(container);

            if (AppSettings.EnableMigrations)
            {
                MigrationsHelper.MigrateToLatestVersion(AppSettings.MongoDbContext);
                InitializeData(container);
                InitialiseConfig(container);
            }
        }

        static void ConfigureNlog(string appInsightsKey)
        {
            if (!string.IsNullOrEmpty(appInsightsKey))
            {
                var config = LogManager.Configuration;
                ConfigurationItemFactory.Default.Targets.RegisterDefinition("ai", typeof(ApplicationInsightsTarget));
                var aiTarget = new ApplicationInsightsTarget
                {
                    InstrumentationKey = appInsightsKey,
                    Name = "ai"
                };
                config.AddTarget("ai", aiTarget);
                config.AddRuleForAllLevels("ai");
                LogManager.Configuration = config; // Even though the objects are the same, this setting is important or else the aiTarget will not be initialised.
            }
        }

        private void ConfigureOAuth(IAppBuilder app, IContainer container)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),
                Provider = container.Resolve<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = container.Resolve<IAuthenticationTokenProvider>()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void InitializeData(IContainer container)
        {
            container.Resolve<MongoContextInitialiser>().Intitialise();
        }

        private void InitialiseAzureStorage(IContainer container)
        {
            container.Resolve<AttachmentStorageClient>().Initialise();
            container.Resolve<ImageStorageClient>().Initialise();
            container.Resolve<ReportStorageClient>().Initialise();
        }

        private void InitialiseQueues(IContainer container)
        {
            container.Resolve<UpdateSubscriptionQueueClient>().Initialise();
            container.Resolve<ActionNotificationQueueClient>().Initialise();
        }

        private void InitialiseConfig(IContainer container)
        {
            var service = container.Resolve<GlobalConfigurationService>();
            service.InitialiseConfig().GetAwaiter().GetResult();
            if (AppSettings.EnableTimeMachine) service.GetTimeMachineConfigAsync().GetAwaiter().GetResult();
        }
    }
}
