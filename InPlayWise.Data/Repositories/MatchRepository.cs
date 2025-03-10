using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InPlayWise.Data.Repositories
{
    public class MatchRepository : IMatchRepository
	{

		private readonly IConfiguration _config;
		private readonly AppDbContext _db;

		public MatchRepository(IConfiguration configuration, AppDbContext appDbContext) { 
			_config = configuration;
			_db = appDbContext;
		}

		public async Task<Result<List<RecentMatchModel>>> GetRecentMatchesOfTeam(string teamId)
		{
			try
			{
				return new Result<List<RecentMatchModel>>()
				{
					Items = await _db.RecentMatches.Where(match => match.HomeTeamId.Equals(teamId) || match.AwayTeamId.Equals(teamId)).OrderByDescending(match => match.MatchStartTimeOfficial).ToListAsync(),
					IsSuccess = true
				};
			} catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<RecentMatchModel>>()
				{
					Items = new List<RecentMatchModel>(),
					IsSuccess = false
				};				
			}
		}


		public async Task<Result<List<RecentMatchModel>>> GetHomeMatchesOfTeam(string teamId)
		{
			try
			{
				return new Result<List<RecentMatchModel>>()
				{
					Items = await _db.RecentMatches.Where(match => match.HomeTeamId.Equals(teamId)).OrderByDescending(match => match.MatchStartTimeOfficial).ToListAsync(),
					IsSuccess = true
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<RecentMatchModel>>()
				{
					Items = new List<RecentMatchModel>(),
					IsSuccess = false
				};
			}
		}

		public async Task<Result<List<RecentMatchModel>>> GetAwayMatchesOfTeam(string teamId)
		{
			try
			{
				return new Result<List<RecentMatchModel>>()
				{
					Items = await _db.RecentMatches.Where(match => match.AwayTeamId.Equals(teamId)).OrderByDescending(match => match.MatchStartTimeOfficial).ToListAsync(),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<RecentMatchModel>>()
				{
					Items = new List<RecentMatchModel>(),
					IsSuccess = false
				};
			}
		}

		public async Task<Result<List<RecentMatchModel>>> GetSeasonGamesOfTeam(string teamId, string seasonId)
		{
			try
			{
				return new Result<List<RecentMatchModel>>()
				{
					Items = await _db.RecentMatches.Where(match => match.HomeTeamId.Equals(teamId) && match.SeasonId.Equals(seasonId)).OrderByDescending(match => match.MatchStartTimeOfficial).ToListAsync(),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<RecentMatchModel>>()
				{
					Items = new List<RecentMatchModel>(),
					IsSuccess = false
				};
			}
		}


		Task<RecentMatchModel> IMatchRepository.GetLiveMatches(string teamId)
		{



			throw new NotImplementedException();



		}


		public async Task<LiveMatchModel> GetLiveMatch(string matchId)
		{
			try
			{
				LiveMatchModel match = await _db.LiveMatches.FindAsync(matchId);
				return match;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}




		public async Task<List<string>> GetHomeAndAwayTeamOfMatch(string matchId)
		{
			try
			{
				var match = (await _db.RecentMatches.Where(match => match.MatchId.Equals(matchId)).FirstOrDefaultAsync());
				if(match is null)
				{
					throw new Exception();
				}
				return new List<string> { match.HomeTeamId, match.AwayTeamId };

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new List<string> { "", "" };
			}
		}

		public async Task<bool> UploadLiveMatches(List<LiveMatchModel> liveMatches)
		{
			try
			{
				await _db.LiveMatches.AddRangeAsync(liveMatches);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}


		public async Task<List<LiveMatchModel>> GetAllLiveMatches()
		{
			try
			{
				return await _db.LiveMatches.ToListAsync();
			}catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new List<LiveMatchModel>();
			}

		}

		public async Task<bool> UploadLiveMatch(LiveMatchModel match)
		{
			try
			{
				await _db.LiveMatches.AddAsync(match);
				await _db.SaveChangesAsync();
				return true;
			} catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> UpdateLiveMatch(LiveMatchModel match)
		{
			try
			{
				var liveMatch = _db.LiveMatches.Find(match.MatchId);
				liveMatch.MatchStatus = match.MatchStatus;
				liveMatch.MatchMinutes = match.MatchMinutes;
				liveMatch.HomeGoals = match.HomeGoals;
				liveMatch.AwayGoals = match.AwayGoals;
				liveMatch.HomeRed = match.HomeRed;
				liveMatch.AwayRed = match.AwayRed;
				liveMatch.HomeYellow = match.HomeYellow;
				liveMatch.AwayYellow = match.AwayYellow;
				liveMatch.HomeCorners = match.HomeCorners;
				liveMatch.AwayCorners = match.AwayCorners;
				liveMatch.HomeShotsOffTarget = match.HomeShotsOffTarget;
				liveMatch.AwayShotsOffTarget = match.AwayShotsOffTarget;
				liveMatch.HomeShotsOnTarget = match.HomeShotsOnTarget;
				liveMatch.AwayShotsOnTarget = match.AwayShotsOnTarget;

				liveMatch.HomeDangerousAttacks = match.HomeDangerousAttacks;
				liveMatch.AwayDangerousAttacks = match.AwayDangerousAttacks;
				liveMatch.HomeAttacks = match.HomeAttacks;
				liveMatch.AwayAttacks = match.AwayAttacks;
				liveMatch.HomePenalties = match.HomePenalties;
				liveMatch.AwayPenalties = match.AwayPenalties;
				liveMatch.HomePossession = match.HomePossession;
				liveMatch.AwayPossession = match.AwayPossession;
				liveMatch.HomeOwnGoals = match.HomeOwnGoals;
				liveMatch.AwayOwnGoals = match.AwayOwnGoals;
				liveMatch.HomeGoalMinutes = match.HomeGoalMinutes;
				liveMatch.AwayGoalMinutes = match.AwayGoalMinutes;
				await _db.SaveChangesAsync();
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}



		public async Task<bool> AddToRecentMatches(List<RecentMatchModel> matches)
		{
			try
			{
				await _db.RecentMatches.AddRangeAsync(matches);
				await _db.SaveChangesAsync();
				return true;
			}catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}


		public async Task<bool> DeleteEndedMatch()
		{
			try
			{
				var matches = await _db.LiveMatches.Where(match => match.MatchStatus == 8).ToListAsync();
				_db.RemoveRange(matches);
				await _db.SaveChangesAsync();
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}




		public async Task<bool> UploadMatches(List<RecentMatchModel> matches)
		{
			try
			{
				await _db.RecentMatches.AddRangeAsync(matches);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}



		public async Task<List<LiveMatchModel>> GetEndedMatches()
		{
			try
			{
				return await _db.LiveMatches.Where(match => match.MatchStatus == 8).ToListAsync();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return new List<LiveMatchModel>();
			}
		}

		public async Task<bool> UploadRecentMatch(RecentMatchModel match)
		{
			try
			{
				if ((await _db.RecentMatches.FindAsync(match.MatchId)) is null)
				{
					await _db.RecentMatches.AddAsync(match);
					await _db.SaveChangesAsync();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> UpdateRecentMatch(RecentMatchModel match)
		{
			try
			{
				var matchToUpdate = await _db.RecentMatches.FindAsync(match.MatchId);
				matchToUpdate = match;
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}



		public async Task<bool> MatchExists(string matchId)
		{
			try
			{
				LiveMatchModel match = await  _db.LiveMatches.FindAsync(matchId);
				return match is not null;
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}


		public async Task<bool> DeleteLiveMatch(string matchId)
		{
			try
			{
				LiveMatchModel match = await _db.LiveMatches.FindAsync(matchId);
				if(match is not null)
				{
					_db.LiveMatches.Remove(match);
					await _db.SaveChangesAsync();
				}
				return true;
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<RecentMatchModel> LiveMatchInfo(string matchId)
		{
			try
			{
				RecentMatchModel match = await _db.RecentMatches.FindAsync(matchId);
				return match;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<LiveMatchModel> LiveMatchStats(string matchId)
		{
			try
			{
				LiveMatchModel match = await _db.LiveMatches.FindAsync(matchId);
				return match;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<List<LiveMatchModel>> GetCompleteLiveMatches()
		{
			try
			{
				List<LiveMatchModel> matches = await _db.LiveMatches.ToListAsync();
				return matches;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}
	}
}