using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class LeagueStatsRepository : ILeagueStatsRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<LeagueStatsRepository> _logger;
        public LeagueStatsRepository(AppDbContext db, ILogger<LeagueStatsRepository> logger) {
            _db = db;
            _logger = logger;
            _db.Database.SetCommandTimeout(3000);
        }

        public async Task<bool> AddLeagueStats(LeagueStats leagueStats)
        {
            try
            {
                await _db.LeagueStats.AddAsync(leagueStats);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteLeagueStats(string leagueId)
        {
            try
            {
                LeagueStats stat = await _db.LeagueStats.FindAsync(leagueId);
                if (stat is null) return true;
                _db.Remove(stat);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<LeagueStats> GetLeagueStatsByLeagueId(string leagueId)
        {
            try
            {
                return await _db.LeagueStats.Include(ls=>ls.Competition).FirstOrDefaultAsync(ls => ls.Competition.Id.Equals(leagueId));
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<LeagueStats>> GetTenLeagueStats()
        {
            try
            {
                List<string> desiredId = new List<string>()
                {
                    "jednm9whz0ryox8", "vl7oqdehlyr510j", "gy0or5jhg6qwzv3", "4zp5rzghp5q82w1", "yl5ergphnzr8k0o", "kjw2r09hv5rz84o", "p3glrw7hzkvqdyj", "9vjxm8ghx2r6odg", "vl7oqdeheyr510j", "kn54qllhg2qvy9d", "p3glrw7hevqdyjv"
                };
                return await _db.LeagueStats.Where(ls => desiredId.Contains(ls.CompetitionId)).Include(ls => ls.Competition).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateLeagueStats(LeagueStats leagueStats)
        {
            try
            {
                LeagueStats stat = await _db.LeagueStats.FindAsync(leagueStats.CompetitionId);
                _db.Entry(stat).CurrentValues.SetValues(leagueStats);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }


        public async Task<List<LeagueStatsCount>> GetHitsRecordOfUser(string userId)
        {
            try
            {
                return await _db.LeagueStatsCounter.Where(lsc => lsc.UserId.Equals(userId)).ToListAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
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


        public async Task<bool> AddLeagueStatsCounter(LeagueStatsCount counter)
        {
            try
            {
                await _db.LeagueStatsCounter.AddAsync(counter);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<List<RecentMatchModel>> GetAllRecentMatchesOfSeason(string seasonId)
        {
            try
            {
                return await _db.RecentMatches.Where(m => m.SeasonId.Equals(seasonId)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<Competition> GetCompetition(string id)
        {
            try
            {
                return await _db.Competitions.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<bool> SetFavouriteLeagueStats(List<FavouriteLeagueStat> leagueStats)
        {
            try
            {
                string userId = leagueStats[0].UserId;
                List<FavouriteLeagueStat> leagues = await _db.FavouriteLeagueStats.Where(m => m.UserId.Equals(userId)).ToListAsync();
                _db.FavouriteLeagueStats.RemoveRange(leagues);
                await _db.FavouriteLeagueStats.AddRangeAsync(leagueStats);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<List<FavouriteLeagueStat>> GetFavouriteLeagueStats(string userId)
        {
            try
            {
                return await _db.FavouriteLeagueStats.Where(fl => fl.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<FavouriteLeagueStat>();
            }
        }
    }
}
