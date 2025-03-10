using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerMinuteOneExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerMinuteOneExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var advertisementService = scope.ServiceProvider.GetRequiredService<IAdvertisementService>();
					var predictionService = scope.ServiceProvider.GetRequiredService<IOpportunitiesPredictionService>();
					//await predictionService.UploadToActiveMatchPrediction();
					await predictionService.SavePredictionRecord();
					var _opPredService = scope.ServiceProvider.GetRequiredService<IOpportunitiesPredictionService>();
					await _opPredService.UpdateInMemoryOpportunities();
					await advertisementService.UpdateInMemoryAds();
				}
				await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
			}
		}
	}
}
