
using Hangfire;
using Naviair.Web.Jobs;
using NaviAir.Core.Config;
using NaviAir.Core.Jobs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfire(configuration =>
    configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("umbracoDbDSN")));

// Add the Hangfire server
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<UsernameWhitelistAuthorizationFilter>();


builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .SetBackOfficeUserManager<CustomBackOfficeUserManager>()
    .Build();

WebApplication app = builder.Build();
AppSettings.AppSettingsConfigure(app.Services.GetRequiredService<IConfiguration>());

await app.BootUmbracoAsync();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { app.Services.GetRequiredService<UsernameWhitelistAuthorizationFilter>() }
});

RecurringJob.RemoveIfExists("PublishJob.Execute");
RecurringJob.AddOrUpdate<PublishDocumentsJob>("PublishDocumentsJob", x => x.Execute(), Cron.Minutely);
RecurringJob.AddOrUpdate<PublishFoldersJob>("PublishFoldersJob", x => x.Execute(), Cron.Minutely);

RecurringJob.RemoveIfExists("UnpublishFoldersJob.Execute");
RecurringJob.RemoveIfExists("UnpublishDocumentsJob.Execute");
RecurringJob.AddOrUpdate<UnpublishJobs>("UnpublishJobs", x => x.Execute(), Cron.Minutely);

app.UseUmbraco()

    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();

    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });
app.UseServerVariablesMiddleware();
await app.RunAsync();
