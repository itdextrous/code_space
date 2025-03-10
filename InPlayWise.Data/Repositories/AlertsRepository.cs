using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{



    public class AlertsRepository : IAlertsRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<AlertsRepository> _logger;
        public AlertsRepository(AppDbContext db, ILogger<AlertsRepository> logger)
        {
            _db = db ;
            _logger = logger ;
        }

        public async Task<bool> AlertExists(MatchAlert alert)
        {
            try
            {
                return (await _db.MatchAlerts.SingleOrDefaultAsync(match => match.UserId.Equals(alert.UserId) && match.MatchId.Equals(alert.MatchId))) is not null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return true;
            }
        }

        public async Task<UpcomingMatch> GetMatch(string matchId)
        {
            try
            {
                return await _db.UpcomingMatches.FindAsync(matchId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



        public async Task<bool> AddAlert(MatchAlert alert)
        {
            try
            {
                await _db.MatchAlerts.AddAsync(alert);
                await _db.SaveChangesAsync();
                return false;

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }



        public async Task<List<MatchAlert>> GetAllAlerts()
        {
            try
            {
                return await _db.MatchAlerts.ToListAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<MatchAlert>> GetTrigerredAlerts()
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                return await _db.MatchAlerts.Where(al => al.AlertTime > utcNow).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public Task<bool> AddOrUpdateAlert(MatchAlert alert)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UpcomingMatch>> GetUpcomingMatches()
        {
            try
            {
                return await _db.UpcomingMatches.Include(um => um.HomeTeam)
                    .Include(um => um.AwayTeam).Include(um => um.Competition).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> DeleteMultipleAlerts(List<MatchAlert> alerts)
        {
            try
            {
                _db.MatchAlerts.RemoveRange(alerts);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
    }
}
