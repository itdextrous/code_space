using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
    public class PerSecondsFourExecutables : BackgroundService
	{

		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerSecondsFourExecutables(IServiceScopeFactory serviceScopeFactory)
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
				await Task.Delay(TimeSpan.FromSeconds(4), stoppingToken);
			}
		}

	}
}
