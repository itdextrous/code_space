using InPlayWise.Core.InMemoryServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;



namespace InPlayWise.Core.Hubs
{
    [Authorize]
    public sealed class SessionHub:Hub
    {
        private readonly IHubContext<SessionHub> _context;
        private readonly MatchInMemoryService _inMemory;
        private readonly IUserSessionRepository _userSessionRepo;
        public SessionHub(IHubContext<SessionHub> hubContext, MatchInMemoryService matchInMemoryService, IUserSessionRepository userSessionRepository)
        {
            _context = hubContext;
            _inMemory = matchInMemoryService;
            _userSessionRepo = userSessionRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userclaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
            var productClaim = Context.User.FindFirst("ProductId");
        
            string userId = userclaim.Value.ToString();
            string productId = productClaim.Value.ToString();
            var userSession = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ConnectedTime = DateTime.UtcNow,
                ProductId = productId
            };

            List<UserSession> _userSessions = _inMemory.GetUserSessions();
             _inMemory.SetUserSessions(_userSessions, userSession);
        }
      
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userSession = _inMemory.GetUserSessions().FirstOrDefault(us => us.UserId.Equals(userId));
            if (userSession != null)
            {
                userSession.DisconnectedTime = DateTime.Now;
                _userSessionRepo.UpdateUserSessionRepo(userSession);
                _inMemory.GetUserSessions().Remove(userSession);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
}
