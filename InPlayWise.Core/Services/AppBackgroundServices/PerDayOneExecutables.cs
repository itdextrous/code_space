using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerDayOneExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerDayOneExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var historicMatchService = scope.ServiceProvider.GetRequiredService<IHistoricMatchBackgroundProcess>();
                    await historicMatchService.CompleteMatchesInfo();
                    await historicMatchService.SeedMatchesInMemory();

                }
				await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
			}
		}
	}
}
