using Azure.Storage.Blobs;
using Chat.Data.IUOW;
using InPlayWise.Core.BackgroundProcess;
using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services;
using InPlayWise.Core.Services.AppBackgroundServices;
using InPlayWise.Data.DbContexts;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.Repositories;
using InPlayWiseCore.IServices;
using InPlayWiseCore.Services;
using InPlayWiseCore.Services.AuthToken;
using InPlayWiseCore.Services.Emails;
using InPlayWiseData.Data;
using InPlayWiseData.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;

namespace InPlayWiseApi.Extensions
{
    public static class ConfigureServices
    {

        /// <summary>
        /// Adds the application's DbContext to the dependency injection container.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        /// <param name="configuration">The configuration containing the connection string.</param>

        public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("localConn")), ServiceLifetime.Scoped);
            }
            if (environment.IsProduction())
            {
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("liveConn")), ServiceLifetime.Scoped);
            }

            services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("localConn")), ServiceLifetime.Scoped);

        }

        /// <summary>
        /// Adds custom application services to the dependency injection container.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        /// <param name="configuration">The configuration containing service-related settings.</param>
        ///
        public static void AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Tokens.ProviderMap["Default"] = new TokenProviderDescriptor(
    typeof(DataProtectorTokenProvider<IdentityUser>));
                opt.SignIn.RequireConfirmedAccount = true ;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            // Transient services
            //services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IEmailServices, EmailServices>();


            // Scoped services
            //services.AddScoped<IPredictionServices, PredictionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenServices, TokenServices>();

            // Identity services (Scoped)
            services.AddHttpClient();
            // AutoMapper (Transient)
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Stripe Configuration (Singleton)
            StripeConfiguration.ApiKey = configuration.GetValue<string>("StripeSettings:SecretKey");
            // Stripe services (Scoped)
            services.AddScoped<CustomerService>()
                    .AddScoped<ChargeService>()
                    .AddScoped<TokenService>()
                    .AddScoped<IAuthRepository, AuthRepository>()
                    .AddScoped<IBasicInfoServices, BasicInfoServices>()
                    .AddScoped<IBasicDataServices, BasicDataServices>()
                    .AddScoped<ICleverLabelServices, CleverLabelServices>()
                    .AddScoped<IMatchRepository, MatchRepository>()
                    .AddScoped<IMatchService, MatchService>()
                    .AddScoped<ILiveMatchService, LiveMatchService>()
                    //.AddScoped<IPredictionRespository, PredictionRepository>()
                    //.AddScoped<IPredictionServices, PredictionService>()
                    .AddScoped<IInsightService, InsightService>()
                    .AddScoped<IInsightsRepository, InsightsRepsitory>()
                    .AddScoped<ICleverLabelsRepository, CleverLabelsRepository>()
                    .AddScoped<ILiveMatchRepository, LiveMatchRepository>()
                    .AddScoped<IHttpContextService, HttpContextService>()
                    .AddScoped<IRecentMatchService, RecentMatchesService>()
                    .AddScoped<IRecentMatchRepository, RecentMatchRepository>()
                    .AddScoped<IProfileService, ProfileService>()
                    .AddScoped<IProfileRepository, ProfileRepository>()
                    .AddScoped<ITeamsRepository, TeamsRepository>()
                    .AddScoped<ITeamsService, TeamsService>()
                    .AddScoped<IOddsDataService, OddsDataService>()
                    .AddScoped<IStripePaymentService, StripePaymentService>()
                    .AddScoped<IMembershipRepository, MembershipRepository>()
                    .AddScoped<IMembershipService, MembershipService>()
                    .AddScoped<ISportsApiDataSeedRepository, SportsApiDataSeedRepository>()
                    .AddScoped<ISportsApiDataSeedService, SportsApiDataSeedService>()
                    .AddScoped<ICompetitionRepository, CompetitionRepository>()
                    .AddScoped<ICompetitionService, CompetitionService>()
                    .AddScoped<IUpcomingMatchesRepository, UpcomingMatchesRepository>()
                    .AddScoped<IUpcomingMatchesService, UpcomingMatchesService>()
                    .AddScoped<IAlertsRepository, AlertsRepository>()
                    .AddScoped<IAlertsService, AlertsService>()
                    .AddScoped<IFeatureCounterRepository, FeatureCounterRepository>()
                    .AddScoped<IFeatureCounterService, FeatureCounterService>()
                    .AddScoped<IOpportunitiesPredictionRepository, OpportunitiesPredictionRepository>()
                    .AddScoped<IOpportunitiesPredictionService, OpportunitiiesPredictionService>()
                    .AddScoped<IShocksRepository, ShocksRepository>()
                    .AddScoped<IShocksService, ShocksService>()
                    .AddScoped<IAdvertisementRepository, AdvertisementRepository>()
                    .AddScoped<IAdvertisementService, AdvertisementService>()
                    .AddScoped<ILeagueStatsRepository, LeagueStatsRepository>()
                    .AddScoped<ILeagueStatsService, LeagueStatsService>()
                    .AddScoped<IHistoricalStatsRepository, HistoricalStatsRepository>()
                    .AddScoped<IHistoricalStatsService, HistoricalStatsService>()
                    .AddScoped<IAccumulatorService, AccumulatorService>()
                    .AddScoped<IAccumulaterRepository, AccumulatorRepository>()
                    .AddScoped<IAdminService, AdminService>()
                    .AddScoped<IAdminRepository, AdminRepository>()
                    .AddScoped<IStripePaymentService, StripePaymentService>()
                    .AddScoped<ILiveMatchBackgroundProcess, LiveMatchBackgroundProcess>()
                    .AddScoped<IHistoricMatchBackgroundProcess, HistoricalMatchBackgroundProcess>()
                    .AddScoped<ITeamsBackgroundProcess, TeamsBackgroundProcess>()
                    .AddScoped<IUserSessionRepository, UserSessionRepository>()
                    .AddScoped<IBannerRepository, BannerRepository>()
                    .AddScoped<IBannerService, BannerService>()
                    .AddScoped<ILocalUserDataRepository, LocalUserDataRepository>()
                    .AddScoped<IUserDataBackgroundProcess, UserDataBackgroundProcess>()
                    .AddSingleton(x => new BlobServiceClient(configuration.GetValue<string>("BlobStorage:StorageAccount")));


            services
            .AddHostedService<PerDayThreeExecutables>()
            .AddHostedService<PerDayOneExecutables>()
            .AddHostedService<PerHourFiveExecutables>()
            .AddHostedService<PerHourOneExecutables>()
            .AddHostedService<PerMinuteThirtyExecutables>()
            .AddHostedService<PerMinuteTenExecutables>()
            .AddHostedService<PerMinuteFiveExecutables>()
            .AddHostedService<PerMinuteOneExecutables>()
            .AddHostedService<PerSecondsTwentyExecutables>()
            .AddHostedService<PerSecondsTenExecutables>()
            .AddHostedService<PerSecondsFourExecutables>()
            ;

            if (environment.IsProduction())
            {
                services
                .AddHostedService<PerDayTwoExecutables>()
                //.AddHostedService<PerMinuteOneExecutables>()
                ;
            }

            services.AddSingleton<MatchInMemoryService>();


            

            services.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "logs - ";
                options.FileSizeLimit = 5000 * 1024;
                options.RetainedFileCountLimit = 100;
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InPlayWise", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id= "Bearer"
                        }
                    },
                    new string[]{}
                    }
                });
            });
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));
            });
        }
    }
}
