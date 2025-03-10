using InPlayWise.Core.BackgroundProcess.Interface;
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
	public class PerSecondsTenExecutables : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public PerSecondsTenExecutables(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{

                    //var livematchservice = scope.ServiceProvider.GetRequiredService<ILiveMatchService>();
                    var liveMatchService = scope.ServiceProvider.GetService<ILiveMatchBackgroundProcess>();
                    //var oppService = scope.ServiceProvider.GetRequiredService<IOpportunitiesPredictionService>();
                    //var membershipService = scope.ServiceProvider.GetRequiredService<IMembershipService>();
                    await liveMatchService.UploadAndUpdateLiveMatches();
                    //await oppService.RemoveEndedOpportunities();
                    //await membershipService.DailyRefreshMembershipStatus();
                    //ILiveMatchService liveMatch2 = scope.ServiceProvider.GetService<ILiveMatchService>();
                    //await liveMatch2.BroadcastMatches();



                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
			}
		}
	}
}
