using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerHourFiveExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerHourFiveExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					//var predictionService = scope.ServiceProvider.GetRequiredService<IOpportunitiesPredictionService>();
					//var historicMatchService = scope.ServiceProvider.GetRequiredService<IHistoricMatchBackgroundProcess>();
					//bool completed = await historicMatchService.CompleteMatchesInfo();
					//var dataSeed = scope.ServiceProvider.GetRequiredService<ISportsApiDataSeedService>();

					//bool done = await predictionService.TrainModelAndUpdateDatabase();
					//bool saved = (await dataSeed.SeedUpcomingMatches());

				}
				await Task.Delay(TimeSpan.FromHours(5), stoppingToken);
			}
		}
	}
}
