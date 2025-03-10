using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerMinuteTenExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerMinuteTenExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{

				}
				await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
			}
		}
	}
}
