using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class CleverLabelsRepository : ICleverLabelsRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<CleverLabelsRepository> _logger;

        public CleverLabelsRepository(AppDbContext db, ILogger<CleverLabelsRepository> logger) {
            _db = db;
            _logger = logger;
        }

        public async Task<List<RecentMatchModel>> GetLastThreeMatchesOfTeam(string teamId)
        {
            try
            {
                return await _db.RecentMatches.Where(match => (match.HomeTeamId.Equals(teamId) || match.AwayTeamId.Equals(teamId)) && match.CompleteInfo).OrderByDescending(match => match.MatchStartTimeOfficial).Take(3).ToListAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<PlanFeatures> GetPlanFeatures(string userId)
        {
            try
            {
                Guid productId = (await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(userId))).ProductId;
                PlanFeatures feature = await _db.PlanFeatures.SingleOrDefaultAsync(ft => ft.ProductId.Equals(productId));
                return feature;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<CleverLabelsCounter>> GetUserHitsOnMatches(string userId)
        {
            try
            {
                return await _db.CleverLabelsCounters.Where(clc => clc.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return null; }
        }

        public async Task<bool> AddCounter(CleverLabelsCounter counter)
        {
            try
            {
                await _db.CleverLabelsCounters.AddAsync(counter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return false; }
        }

        public async Task<bool> UpdateCounter(PredictionCounter counter)
        {
            try
            {
                PredictionCounter exCounter = await _db.PredictionCounters.FindAsync(counter.Id);
                _db.Entry(exCounter).CurrentValues.SetValues(counter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return false; }
        }

        public async Task<List<LiveMatchModel>> GetAllLiveMatches()
        {
            try
            {
                return await _db.LiveMatches.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

		public async Task<List<RecentMatchModel>> GetAllMatchesOfSeason(string seasonId)
		{
            try
            {
                return await _db.RecentMatches.Where(rm => rm.SeasonId.Equals(seasonId)).ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
		}

		public async Task<List<RecentMatchModel>> GetPastMatchesOfTeam(string teamId)
		{
            try
			{
				return await _db.RecentMatches.Where(rm => (rm.HomeTeamId.Equals(teamId) || rm.AwayTeamId.Equals(teamId)) && rm.CompleteInfo).OrderByDescending(rm => rm.MatchStartTimeOfficial).ToListAsync();
			}catch(Exception ex)
            {
				_logger.LogError(ex.ToString());
				return null;
			}
		}
	}
}
