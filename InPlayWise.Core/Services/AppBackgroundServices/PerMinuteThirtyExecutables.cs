using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerMinuteThirtyExecutables : BackgroundService
	{

		private readonly IServiceScopeFactory _serviceScopeFactory;
		public PerMinuteThirtyExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					//var upcomingMatchService = scope.ServiceProvider.GetRequiredService<IUpcomingMatchesService>();
					//await upcomingMatchService.DeleteOldMatches();
				}
				await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
			}
		}
	}
}
