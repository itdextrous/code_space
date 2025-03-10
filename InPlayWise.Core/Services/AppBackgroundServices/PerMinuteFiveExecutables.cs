using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
    public class PerMinuteFiveExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerMinuteFiveExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					//var liveMatchService = scope.ServiceProvider.GetRequiredService<ILiveMatchBackgroundProcess>();
					var upcomingMatchService = scope.ServiceProvider.GetRequiredService<IUpcomingMatchesService>();
					
					//await liveMatchService.deleteFinishedMatches();
					await upcomingMatchService.SeedUpcomingMatchesInMemory();
					var membershipService = scope.ServiceProvider.GetRequiredService<IMembershipService>();
					await membershipService.DailyRefreshMembershipStatus();

                    //var liveMatchService = scope.ServiceProvider.GetService<ILiveMatchBackgroundProcess>();
                    //var oppService = scope.ServiceProvider.GetRequiredService<IOpportunitiesPredictionService>();
                    //var membershipService = scope.ServiceProvider.GetRequiredService<IMembershipService>();
                    //await liveMatchService.UploadAndUpdateLiveMatches();
                    //await oppService.RemoveEndedOpportunities();
					//await liveMatchService.RefreshFilters();

				}
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
			}
		}
	}
}
