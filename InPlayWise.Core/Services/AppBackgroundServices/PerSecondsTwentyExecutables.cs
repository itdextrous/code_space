using InPlayWise.Core.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Core.Services.AppBackgroundServices
{
	public class PerSecondsTwentyExecutables : BackgroundService
	{

		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerSecondsTwentyExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					//var _opPredService = scope.ServiceProvider.GetRequiredService<IOpportunitiesPredictionService>();
					//await _opPredService.UpdateInMemoryOpportunities();

				}
				await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
			}
		}

	}
}
