using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerDayThreeExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerDayThreeExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var recentMatchService = scope.ServiceProvider.GetRequiredService<IRecentMatchService>();
					//await recentMatchService.CompleteRecentMatches();
					//await recentMatchService.CompleteRecentMatches();
				}
				await Task.Delay(TimeSpan.FromDays(2), stoppingToken);
			}
		}
	}
}
