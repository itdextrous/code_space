using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerDayTwoExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
		

		public PerDayTwoExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					//var historicalMatchProcess = scope.ServiceProvider.GetRequiredService<IHistoricMatchBackgroundProcess>();
					//await historicalMatchProcess.CompleteMatchesInfo();
					//await historicalMatchProcess.SeedMatchesInMemory();

				}
				await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
			}
		}
	}
}
