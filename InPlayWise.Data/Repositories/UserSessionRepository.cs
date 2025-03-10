using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
	public class UserSessionRepository : IUserSessionRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<UserSessionRepository> _logger;


        public UserSessionRepository(AppDbContext db, ILogger<UserSessionRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> UpdateUserSessionRepo(UserSession userSession)
        {
            await _db.UserSessions.AddAsync(userSession);
             await  _db.SaveChangesAsync();
            return true;
        }

        
        
    }
}
