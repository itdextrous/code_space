using InPlayWise.Core.Hubs;
using InPlayWiseApi.Extensions;
using InPlayWiseApi.Middlewares;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Custom services.
builder.Services.AddAppDbContext(builder.Configuration, builder.Environment);
builder.Services.AddServices(builder.Configuration, builder.Environment);


builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
//builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));


if (builder.Environment.IsProduction())
{
    StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings:SecretKey").Value;
}
if (builder.Environment.IsDevelopment())
{
    StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings:SecretKey").Value;
}


var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseMiddleware<WebSocketsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.SeedData();

app.MapHub<SessionHub>("/hub/session");
app.MapHub<AlertsHub>("/hub/alerts");
app.MapHub<MatchesHub>("/hub/matches");


app.Run();