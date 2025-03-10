using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class HistoricalStatsRepository : IHistoricalStatsRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HistoricalStatsRepository> _logger;
        public HistoricalStatsRepository(AppDbContext db, ILogger<HistoricalStatsRepository> logger) {
            _logger = logger;
            _db = db;
        }


        public async Task<List<RecentMatchModel>> GetLastMatchesOfTeam(string teamId)
        {
            try
            {
                return (await _db.RecentMatches.Where(rm => (rm.HomeTeamId.Equals(teamId) || rm.AwayTeamId.Equals(teamId)) && (rm.Ended && !rm.AbruptEnd)).OrderByDescending(rm => rm.MatchStartTimeOfficial).Take(3).ToListAsync());
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<LiveMatchModel> GetLiveMatchById(string matchId)
        {
            try
            {
                return (await _db.LiveMatches.FindAsync(matchId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> GetPastMatchesOfTeam(string teamId)
        {
            try
            {
                return await _db.RecentMatches
                    .Include(rm => rm.HomeTeam)
                    .Include(rm => rm.AwayTeam)
                    .Include(rm => rm.Competition)
                    .Where(rm => (rm.HomeTeamId.Equals(teamId) || rm.AwayTeamId.Equals(teamId)) && rm.MatchStartTimeOfficial > DateTime.UtcNow.AddDays(-30.5))
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<UpcomingMatch> GetUpcomingMatchById(string matchId)
        {
            try
            {
                return (await _db.UpcomingMatches.FindAsync(matchId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
