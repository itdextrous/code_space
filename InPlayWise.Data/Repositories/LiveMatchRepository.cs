using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseData.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InPlayWise.Data.Repositories
{
    public class LiveMatchRepository : ILiveMatchRepository
    {
        private readonly AppDbContext _db;
        public LiveMatchRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<LiveMatchModel>> GetAllLiveMatches()
        {
            try
            {
                return (await _db.LiveMatches.Include(lm => lm.HomeTeam).Include(lm => lm.AwayTeam).Include(lm => lm.Competition).OrderByDescending(match => match.MatchStartTimeOfficial).ToListAsync());                    
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<List<LiveMatchModel>> GetLiveMatchByCompetition(string competitionId)
        {
            try
            {
                return (await _db.LiveMatches.Where(match => match.CompetitionId.Equals(competitionId)).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<LiveMatchModel> GetLiveMatchById(string matchid)
        {
            try
            {
                return (await _db.LiveMatches.FindAsync(matchid));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<LiveMatchModel> GetLiveMatchByTeam(string teamId)
        {
            try
            {
                return (await _db.LiveMatches.FirstOrDefaultAsync(match => match.HomeTeamId.Equals(teamId) || match.AwayTeamId.Equals(teamId)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public async Task<List<string>> GetExistingMatchesId()
        {
            try
            {
                return await _db.LiveMatches.Select(match => match.MatchId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> MatchExists(string matchId)
        {
            try
            {
                var match = await _db.LiveMatches.FindAsync(matchId);
                return match is null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateLiveMatch(LiveMatchModel match)
        {
            try
            {

                var exMatch = await _db.LiveMatches.FindAsync(match.MatchId);
                if (exMatch is null)
                    return false;
                _db.Entry(exMatch).CurrentValues.SetValues(match);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> AddLiveMatch(LiveMatchModel match)
        {
            try
            {
                _db.LiveMatches.Add(match);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<LiveMatchModel>> GetMatchesByStatus(List<int> status)
        {
            try
            {
                return await _db.LiveMatches.Where(match => status.Contains(match.MatchStatus)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> AddToRecentMatch(RecentMatchModel match)
        {
            try
            {
                if ((await _db.RecentMatches.FindAsync(match.MatchId)) is not null)
                    return false;
                await _db.RecentMatches.AddAsync(match);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }




        }

        public async Task<bool> DeleteLiveMatchById(string matchId)
        {
            try
            {
                LiveMatchModel match = await _db.LiveMatches.FindAsync(matchId);
                _db.LiveMatches.Remove(match);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<Team> GetTeamById(string teamId)
        {
            try
            {
                return await _db.Team.FindAsync(teamId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<Competition> GetCompetitionById(string compId)
        {
            try
            {
                return await _db.Competitions
                    .Include(comp => comp.CompetitionCategories)
                    .FirstOrDefaultAsync(comp => comp.Id == compId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<LiveMatchModel>> SearchLiveMatchesByQuery(string query)
        {
            try
            {
                return await _db.LiveMatches
                    .Include(lm => lm.HomeTeam)
                    .Include(lm => lm.AwayTeam)
                    .Include(lm => lm.Competition)
                    .Where(lm => lm.HomeTeam.Name.Contains(query) || lm.AwayTeam.Name.Contains(query) || lm.Competition.Name.Contains(query)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<UpcomingMatch>> SearchUpcomingMatchesByQuery(string query)
        {
            try
            {
                return await _db.UpcomingMatches.Where(um => um.HomeTeam.Name.Contains(query) || um.AwayTeam.Name.Contains(query))
                    .Include(um => um.HomeTeam)
                    .Include(um => um.AwayTeam)
                    .Include(um => um.Competition)
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
                return await _db.RecentMatches.Where(rm => (rm.HomeTeam.Name.Contains(query) || rm.AwayTeam.Name.Contains(query)) && (rm.Ended && !rm.AbruptEnd)).OrderByDescending(rm => rm.MatchStartTimeOfficial).ToListAsync();
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
				var exMatch = await _db.RecentMatches.FindAsync(match.MatchId);
				if (exMatch is null)
					return false;
				_db.Entry(exMatch).CurrentValues.SetValues(match);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<bool> UpdateLiveMatchesRange(List<LiveMatchModel> matches)
		{
            try
            {
                List<LiveMatchModel> matchesInDb = await _db.LiveMatches.ToListAsync();
                foreach(LiveMatchModel match in matches)
                {
                    foreach(var matchModel in matchesInDb)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
		}

        public async Task<List<string>> SearchTeamName(string query)
        {
            try
            {
                return await _db.Team
                    .Where(team => team.Name.ToLower()
                    .Contains(query.ToLower()))
                    .Select(team => team.Id)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> GetMatchesById(List<string> teamIds)
        {
            try
            {
                return await _db.RecentMatches
                    .Where(rm => teamIds.Contains(rm.HomeTeamId) || teamIds.Contains(rm.AwayTeamId))
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                return null;
            }
        }

        public async Task<List<RecentMatchModel>> SearchRecentMatches(string query)
        {
            try
            {
                string sql = @"
                    SELECT RM.*, HT.*, AT.*, C.*
                    FROM RecentMatchModels RM
                    INNER JOIN Teams HT ON RM.HomeTeamId = HT.TeamId
                    INNER JOIN Teams AT ON RM.AwayTeamId = AT.TeamId
                    INNER JOIN Competitions C ON RM.CompetitionId = C.CompetitionId
                    WHERE RM.HomeTeamName like '%@query%' or RM.AwayTeamName like '%query%'";
                SqlParameter param = new SqlParameter("@query", query);
                List<RecentMatchModel> recentMatches = await _db.RecentMatches
                    .FromSqlRaw(sql, param)
                    .ToListAsync();

                return recentMatches;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }
        }
    }
}
