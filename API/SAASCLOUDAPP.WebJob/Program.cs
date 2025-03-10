using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.NLogTarget;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using SAASCLOUDAPP.BackgroundProcessing;
using SAASCLOUDAPP.BusinessLayer;
using SAASCLOUDAPP.BusinessLayer.Services;
using SAASCLOUDAPP.BusinessLayer.Services.Interfaces;
using SAASCLOUDAPP.DataAccessLayer;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using Stripe;
using Workfacta;
using Workfacta.Data;
using Workfacta.Logic;
using Workfacta.Logic.Configuration;
using Workfacta.Logic.Services.Queues;
using Workfacta.Models.Integrations.Xero;
using Workfacta.Shared.Providers;
using Workfacta.Shared.Services;
using Workfacta.Shared.Services.Storage;
using Workfacta.Templating.Services;

namespace SAASCLOUDAPP.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static async Task Main()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(ConfigurationManager.AppSettings["DefaultThreadLocale"]);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var appInsightsKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
            var appInsightsConnStr = ConfigurationManager.AppSettings["APPLICATIONINSIGHTS_CONNECTION_STRING"];
            ConfigureNlog(appInsightsKey);

            var builder = new HostBuilder();
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:AzureWebJobsDashboard", ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString },
                    { "ConnectionStrings:AzureWebJobsStorage", ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString },
                    { "Queues:UpdateSubscription", AppSettings.UpdateSubscriptionQueueName },
                    { "Queues:ActionNotification", AppSettings.ActionNotificationQueueName },
                });
            });
            builder.ConfigureLogging(b =>
            {
                b.AddConsole();
                if (!string.IsNullOrEmpty(appInsightsConnStr))
                {
                    b.AddApplicationInsightsWebJobs(o =>
                    {
                        o.ConnectionString = appInsightsConnStr;
                    });
                }
            });
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddTimers();
                b.AddAzureStorage(queueConfig =>
                {
                    queueConfig.VisibilityTimeout = TimeSpan.FromSeconds(30); // 30 seconds between retries
                });
            });
            builder.ConfigureServices(ConfigureServices);
            var host = builder.Build();

            using (host)
            {
                await host.RunAsync();
            }
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoContext, MongoContext>(config => new MongoContext(AppSettings.MongoDbContext));
            services.AddSingleton<IWorkfactaContext, WorkfactaContext>(config => new WorkfactaContext(AppSettings.MongoDbContext));
            services.AddSingleton<IStripeClient>(context => new StripeClient(AppSettings.StripeApiKey));
            services.AddSingleton(config => new UpdateSubscriptionQueueClient(AppSettings.QueueStorageConnectionString, AppSettings.UpdateSubscriptionQueueName));
            services.AddSingleton(config => new ActionNotificationQueueClient(AppSettings.QueueStorageConnectionString, AppSettings.ActionNotificationQueueName));

            services.AddSingleton(config => new EmailConfiguration
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

                BlacklistedDomains = config.GetService<EmailConfig>()?.BlacklistedDomains
            });

            services.AddSingleton(config => new ContentBuilderConfiguration
            {
                SupportEmail = AppSettings.WorkfactaSupportEmail,
                BaseWebUrl = AppSettings.BaseWebUrl
            });

            services.AddSingleton(config => AutoMapperConfig.Build());
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IMeetingService, MeetingService>();
            services.AddSingleton<IDateProvider, DefaultDateProvider>();
            services.AddSingleton<IDateProviderFactory, DefaultDateProviderFactory>();
            services.AddSingleton<IClientIpAddressAccessor, NoClientIpAddressAccessor>();
            services.AddSingleton<IRecurringActionService, RecurringActionService>();
            services.AddSingleton<IPdfGenerator>(x => null);
            services.AddSingleton<ApplicationUserManager>(x => null);

            services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddScoped<RoleManager>();
            services.AddScoped<StripeService>();
            services.AddScoped<EventHistoryRepository>();
            services.AddScoped<PaymentRepository>();
            services.AddScoped<ExternalDataRetrieval>();
            services.AddSingleton<INewsService, NewsService>();
            services.AddSingleton<Workfacta.Data.Repositories.Interfaces.INewsRepository, Workfacta.Data.Repositories.NewsRepository>();
            services.AddScoped<ChartingService>();
            services.AddScoped<MeetingRepository>();
            services.AddScoped<TeamRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<MeetingReminderRepository>();
            services.AddScoped<GoalsRepository>();
            services.AddScoped<NumbersRepository>();
            services.AddScoped<DaysRepository>();
            services.AddScoped<ActionRepository>();
            services.AddScoped<IssuesRepository>();
            services.AddScoped<GoalStatusRepository>();
            services.AddScoped<PermissionRepository>();
            services.AddScoped<AuthRepository>();
            services.AddScoped<TokenRepository>();
            services.AddScoped<PartnerRepository>();
            services.AddScoped<WorkCategoryRepository>();
            services.AddScoped<FirebaseRepository>();
            services.AddScoped<LinksRepository>();
            services.AddScoped<TacticsRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<EmailGenerationService>();
            services.AddScoped<ContentBuilder>();

            services.AddSingleton(config => new XeroConfigurationClientInfo
            {
                ClientId = AppSettings.XeroClientId,
                ClientSecret = AppSettings.XeroClientSecret
            });
            services.AddScoped<XeroAuthManagerBuilder>();
            services.AddScoped<IXeroDataSourceBuilder, XeroDataSourceBuilder>();

            services.AddAzureStorage(new AzureStorageConfiguration(
                fileStorageConnectionString: AppSettings.FileStorageConnectionString,
                attachmentContainerName: AppSettings.AttachmentContainerName,
                imageContainerName: AppSettings.ImageContainerName,
                reportContainerName: AppSettings.ReportContainerName
            ));
            services.AddWorkfactaRepositories();
            services.AddWorkfactaLogic();
            services.AddWorkfactaModel();
        }

        static void ConfigureNlog(string appInsightsKey)
        {
            var config = new LoggingConfiguration();

            if (!string.IsNullOrEmpty(appInsightsKey))
            {
                ConfigurationItemFactory.Default.Targets.RegisterDefinition("ai", typeof(ApplicationInsightsTarget));
                config.AddTarget("ai", new ApplicationInsightsTarget
                {
                    InstrumentationKey = appInsightsKey,
                    Name = "ai"
                });
                config.AddRuleForAllLevels("ai");
            }

#if DEBUG
            config.AddRuleForAllLevels(new NLog.Targets.DebuggerTarget());
#endif

            LogManager.Configuration = config;
        }
    }
}
