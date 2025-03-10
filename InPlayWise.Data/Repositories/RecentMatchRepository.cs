using InPlayWise.Data.DbContexts;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class RecentMatchRepository : IRecentMatchRepository
    {

        private readonly AppDbContext _db;
        private readonly LocalDbContext _localDb;
        private readonly ILogger<RecentMatchRepository> _logger;

        public RecentMatchRepository(AppDbContext db, LocalDbContext localDb, ILogger<RecentMatchRepository> logger)
        {
            _db = db;
			_db.Database.SetCommandTimeout(3000);
			_localDb = localDb;
            _logger = logger;
        }

        public async Task<List<RecentMatchModel>> GetLast50Matches()
        {
            try
            {
                return await _db.RecentMatches.OrderByDescending(match => match.MatchStartTimeOfficial).Take(50).ToListAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



        public async Task<RecentMatchModel> GetMatchById(string matchId)
        {
            try
            {
                return await _db.RecentMatches
                    .Include(rm => rm.HomeTeam)
                    .Include(rm => rm.AwayTeam)
                    .Include(rm => rm.Competition)
                    .FirstOrDefaultAsync(rm => rm.MatchId == matchId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> GetMatchesByCompetition(string competitionId)
        {
            try
            {
                return await _db.RecentMatches.
                    Where(match => match.CompetitionId.Equals(competitionId)).
                    OrderByDescending(match => match.MatchStartTimeOfficial).
                    Take(50).
                    ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> GetMatchesOfTeamById(string teamId)
        {
            try
            {
                return await _db.RecentMatches.
                    Include(rm => rm.HomeTeam)
                    .Include(rm => rm.AwayTeam)
                    .Include(rm => rm.Competition)
                    .Where(match => (match.AwayTeamId.Equals(teamId) || match.HomeTeamId.Equals(teamId)))
                    .OrderByDescending(match => match.MatchStartTimeOfficial)
                    .Take(50)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        public async Task<List<RecentMatchModel>> GetLastNMatchesOfTeam(string teamId, int n)
        {
            try
            {
                return await _db.RecentMatches.
                    Where(match => (match.AwayTeamId.Equals(teamId) || match.HomeTeamId.Equals(teamId)) && match.CompleteInfo)
                    .OrderByDescending(match => match.MatchStartTimeOfficial)
                    .Take(n)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> SearchRecentMatchesByQuery(string query)
        {
            try
            {
                return await _db.RecentMatches.Where(rm => (rm.HomeTeam.Name.Contains(query) || rm.AwayTeam.Name.Contains(query)) && rm.CompleteInfo).OrderByDescending(rm => rm.MatchStartTimeOfficial).Take(10).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> GetAllMatches()
        {
            try
            {
                //return await _db.RecentMatches.Where(rm => rm.MatchStartTimeOfficial > DateTime.UtcNow.AddDays(-32)).ToListAsync();
                return await _db.RecentMatches.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> UpdateAllMatches(List<RecentMatchModel> matches)
        {
            try
            {
                List<RecentMatchModel> matchesInDb = await _db.RecentMatches.ToListAsync();
                foreach (RecentMatchModel match in matches)
                {
                    foreach (var matchModel in matchesInDb)
                    {
                        if (match.MatchId.Equals(matchModel.MatchId))
                        {
                            _db.Entry(matchModel).CurrentValues.SetValues(match);
                        }
                    }
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

		public async Task<List<RecentMatchModel>> GetThirtyDaysCompletedMatches()
		{
			try
			{
                return await _db.RecentMatches.
                    Where(match => (match.MatchStartTimeOfficial > DateTime.UtcNow.AddDays(-30.5)) && (match.HomeTeamId != null) && match.AwayTeamId != null && (!string.IsNullOrEmpty(match.CompetitionId)) && match.Ended && !match.AbruptEnd)
                    //.OrderByDescending(match => match.MatchStartTimeOfficial)
                    .ToListAsync();
                //return await _db.RecentMatches.ToListAsync();
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<bool> UpdateRecentMatch(RecentMatchModel match)
		{
            try
            {
                RecentMatchModel dbMatch = await _db.RecentMatches.FindAsync(match.MatchId);
				if (dbMatch is null)
					return false;
				_db.Entry(dbMatch).CurrentValues.SetValues(match);
				await _db.SaveChangesAsync();
				return true;
			}
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
		}

		public async Task<bool> UpdateIncompleteMatches(List<RecentMatchModel> matches)
		{
			try
            {
                List<string> existingIds = matches.Select(match => match.MatchId).ToList();

                List<RecentMatchModel> dbMatches = await _db.RecentMatches.Where(m => existingIds.Contains(m.MatchId)).Take(5000).ToListAsync();

                foreach(RecentMatchModel dbMatch in dbMatches)
                {
                    foreach(RecentMatchModel match in matches)
                    {
                        if (dbMatch.MatchId.Equals(match.MatchId))
                        {
							_db.Entry(dbMatch).CurrentValues.SetValues(match);
							break;
                        }
                    }
                }
				await _db.SaveChangesAsync();
                return true;

			}
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
			}
		}

		public async Task<List<RecentMatchModel>> GetIncompleteMatches()
		{
            try
            {
                //return _db.RecentMatches.Where(rm => rm.MatchStartTimeOfficial >= DateTime.UtcNow.AddDays(-28) && rm.MatchStartTimeOfficial <= DateTime.UtcNow.AddDays(-2) && rm.CompletionLevel != 1).ToListAsync();
                //return _db.RecentMatches.Where(rm => rm.CompletionLevel != 1).ToListAsync();
                return await _db.RecentMatches.Where(rm => string.IsNullOrEmpty(rm.MatchId) || string.IsNullOrEmpty(rm.HomeTeamId) || string.IsNullOrEmpty(rm.AwayTeamId) || string.IsNullOrEmpty(rm.CompetitionId) || rm.MatchStartTimeOfficial == DateTime.MinValue).ToListAsync();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
		}

        public async Task<List<string>> GetAllRecentMatchesId()
        {
            try
            {
                return await _db.RecentMatches.Select(m => m.MatchId).OrderBy(matchId=>matchId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<bool> SaveRecentMatches(List<RecentMatchModel> matches)
        {
            try
            {
                await _db.RecentMatches.AddRangeAsync(matches);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<List<RecentMatchModel>> GetMatchesWithoutName()
        {
            try
            {
                //return await _db.RecentMatches.Where(m => string.IsNullOrEmpty(m.HomeTeamName) || string.IsNullOrEmpty(m.AwayTeamName) || string.IsNullOrEmpty(m.CompetitionName)).Take(5000).ToListAsync();
                return await _db.RecentMatches.Where(m => string.IsNullOrEmpty(m.HomeTeam.Name) || string.IsNullOrEmpty(m.HomeTeam.Name) || string.IsNullOrEmpty(m.HomeTeam.Name)).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<Team>> GetAllTeams()
        {
            try
            {
                return await _db.Team.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<Competition>> GetAllCompetitions()
        {
            try
            {
                return await _db.Competitions.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<bool> DeleteFutureMatches()
        {
            try
            {
                List<RecentMatchModel> matchesToDelete = await _db.RecentMatches.Where(m => m.MatchStartTimeOfficial >  DateTime.UtcNow.AddMinutes(30)).ToListAsync();
                _db.RecentMatches.RemoveRange(matchesToDelete);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

		public async Task<List<RecentMatchModel>> LocalDbGetAllMatches()
		{
            try
            {
                return await _localDb.RecentMatches.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
		}

		public async Task<bool> LocalDbUploadMatches(List<RecentMatchModel> matches)
		{
			try
			{
                foreach (var match in matches)
                {
                    try
                    {
                        await _localDb.RecentMatches.AddAsync(match);
                        await _localDb.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        _logger.LogError($"exception occurred for recent match with id {match.MatchId}");
                    }
                }
                //await _localDb.RecentMatches.AddRangeAsync(matches);
                //await _localDb.SaveChangesAsync();
                return true ;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<bool> DeleteMatches(List<string> ids)
		{
			try
			{
                List<RecentMatchModel> matches = await _db.RecentMatches.Where(rm => ids.Contains(rm.MatchId)).ToListAsync();
                _db.RemoveRange(matches);
                await _db.SaveChangesAsync();
                return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<List<RecentMatchModel>> GetMatchesWithId(List<string> ids)
		{
			try
			{
				return await _db.RecentMatches.Where(rm => ids.Contains(rm.MatchId)).ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<bool> AddMatches(List<RecentMatchModel> matches)
		{
			try
			{
                await _db.RecentMatches.AddRangeAsync(matches);
                await _db.SaveChangesAsync();
                return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}
	}
}
