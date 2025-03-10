using InPlayWise.Data.Entities;
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
    public class InsightsRepsitory : IInsightsRepository
	{
		private readonly AppDbContext _db;
		private readonly ILogger<InsightsRepsitory> _logger;

		public InsightsRepsitory(AppDbContext db, ILogger<InsightsRepsitory> logger) {
			_db = db;
			_logger = logger;
		}

		public async Task<List<RecentMatchModel>> GetMatchesByNum( string teamId, int n = 1)
		{
			try
			{
				var matches = await _db.RecentMatches.Where(match => match.HomeTeamId.Equals(teamId) || match.AwayTeamId.Equals(teamId)).OrderBy(obj => obj.MatchStartTimeOfficial).Take(n).ToListAsync();
				return matches;
			}catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<List<RecentMatchModel>> GetLastNMatchesOfTeamAsHome(string teamId, int n = 1)
		{
			try
			{
				var matches = await _db.RecentMatches.Where(match => match.HomeTeamId.Equals(teamId)).OrderByDescending(obj => obj.MatchStartTimeOfficial).Take(n).ToListAsync();
				return matches;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<List<RecentMatchModel>> GetLastNMatchesOfTeamAsAway(string teamId, int n = 1)
		{
			try
			{
				var matches = await _db.RecentMatches.Where( match => match.AwayTeamId.Equals(teamId)).OrderByDescending(obj => obj.MatchStartTimeOfficial).Take(n).ToListAsync();
				return matches;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public Task<List<RecentMatchModel>> GetMatchesBySeason(string teamId, string seasonId = "")
		{
			throw new NotImplementedException();
		}

		public Task<List<RecentMatchModel>> GetMatchesByTime(string teamId, int time = 0)
		{
			throw new NotImplementedException();
		}

		public async Task<List<RecentMatchModel>> GetLastNMatchesOfTeam(string teamId, int n = 10)
		{
			try
			{
				return await _db.RecentMatches.Where(match => match.HomeTeamId.Equals(teamId) || match.AwayTeamId.Equals(teamId)).OrderByDescending(match => match.MatchStartTimeOfficial).Take(n).ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		public async Task<bool> AddInsights(Insights insights)
		{
			try
			{
				await _db.AddAsync(insights);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false ;
			}
		}

		public async Task<bool> UpdateInsights(Insights insights)
		{
			try
			{
				//Insights exInsight = await _db.Insights.FindAsync(insights.Id);
				//if (exInsight is null)
				//	return false;
				//_db.Entry(exInsight).CurrentValues.SetValues(insights);
				//await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
			}
		}

		public Task<Insights> GetInsightsOfTeam(string teamId)
		{
			try
			{
				//return _db.Insights.SingleOrDefaultAsync(insight => insight.TeamId.Equals(teamId));
				return null;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		public async Task<List<Insights>> GetInsightsOfMatch(string matchId)
		{
			try
			{
				//return await _db.Insights.Where(ins => ins.MatchId.Equals(matchId)).ToListAsync();
				return null;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<LiveMatchModel> GetLiveMatchById(string matchId)
		{
			try
			{
				return await _db.LiveMatches.FindAsync(matchId);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<bool> AddHit(LiveInsightsPerGame counter)
		{
			try
			{
				LiveInsightsPerGame count = await _db.LiveInsightsCounter.SingleOrDefaultAsync(ins => ins.UserId.Equals(counter.UserId) && ins.MatchId.Equals(counter.MatchId));
				if(count is null)
				{
					await _db.LiveInsightsCounter.AddAsync(counter) ;
					await _db.SaveChangesAsync() ;
					return true ;
				}
				await _db.SaveChangesAsync();
				return true ;
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<List<Insights>> GetAllInsights()
		{
			try
			{
				//return await _db.Insights.ToListAsync();
				return new List<Insights>();
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<List<LiveMatchModel>> GetAllLiveMatches()
		{
			try
			{
				return (await _db.LiveMatches.OrderByDescending(match => match.MatchStartTimeOfficial).ToListAsync());
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<List<string>> GetAllInsightsMatchId()
		{
			try
			{
				return (await _db.LiveMatches.Select(ins => ins.MatchId).Distinct().ToListAsync());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<bool> RemoveAndUpdateAllInsights(List<Insights> insights)
		{
			try
			{
				//List<Insights> existingInsights = await _db.Insights.ToListAsync();
				//_db.Insights.RemoveRange(existingInsights);
				//await _db.SaveChangesAsync();
				//await _db.Insights.AddRangeAsync(insights);
				//await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
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

		public async Task<LiveInsightsPerGame> GetUserHitsOnMatch(string userId, string matchId)
		{
			try
			{
				LiveInsightsPerGame counter = await _db.LiveInsightsCounter.SingleOrDefaultAsync(c => c.UserId.Equals(userId) && c.MatchId.Equals(matchId));
				if (counter is null) return new LiveInsightsPerGame() { Hits = -1 };
				return counter;
			}catch(Exception ex) { _logger.LogError(ex.ToString()); return null; }
		}

		public async Task<bool> AddCounter(LiveInsightsPerGame counter)
		{
			try
			{
				await _db.AddAsync(counter);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex) { _logger.LogError(ex.ToString()); return false; }
		}

		public async Task<bool> UpdateCounter(LiveInsightsPerGame counter)
		{
            try
            {
                LiveInsightsPerGame exCounter =  await _db.LiveInsightsCounter.FindAsync(counter.Id);
                _db.Entry(exCounter).CurrentValues.SetValues(counter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return false; }
        }
	}
}
