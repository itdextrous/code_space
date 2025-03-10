using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InPlayWise.Core.Hubs
{
	[Authorize]
	public sealed class AlertsHub : Hub
	{

		private readonly MatchInMemoryService _inMemory;

		private readonly ILogger<AlertsHub> _logger;
		public AlertsHub( MatchInMemoryService matchInMemoryService, IUserSessionRepository userSessionRepository, ILogger<AlertsHub> logger)
		{
			_inMemory = matchInMemoryService;
			_logger = logger;
		}

		public override async Task OnConnectedAsync()
		{
			var userclaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
			var productClaim = Context.User.FindFirst("ProductId");
			string userId = userclaim.Value.ToString();
			var userSession = new UserSession
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				ConnectedTime = DateTime.UtcNow,
			};
			_inMemory.GetAlertsUsers().Add(userSession);
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var userAlert = _inMemory.GetAlertsUsers().FirstOrDefault(us => us.UserId.Equals(userId));
			if (userAlert is not null)
			{
				_inMemory.GetAlertsUsers().Remove(userAlert);
			}
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendAlert(string userId, AlertDto alert)
		{
			try
			{
				await Clients.User(userId).SendAsync("ReceiveAlerts", alert);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
			}
		}


	}
}
