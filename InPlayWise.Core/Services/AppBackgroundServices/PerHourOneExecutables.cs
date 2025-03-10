using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerHourOneExecutables : BackgroundService
	{

		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerHourOneExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var dataSeedService = scope.ServiceProvider.GetRequiredService<ISportsApiDataSeedService>();
					await dataSeedService.SeedCompetitions();
					await dataSeedService.SeedTeams();
					await dataSeedService.SeedUpcomingMatches();
					await dataSeedService.SeedTeamCount();
                    var upcomingMatchService = scope.ServiceProvider.GetRequiredService<IUpcomingMatchesService>();
                    await upcomingMatchService.DeleteOldMatches();

                }
				await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
			}
		}
	}
}
