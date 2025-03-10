using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface ILiveMatchRepository
    {
        Task<List<LiveMatchModel>> GetAllLiveMatches();
        Task<LiveMatchModel> GetLiveMatchById(string matchid);
        Task<List<LiveMatchModel>> GetLiveMatchByCompetition(string competitionId);
        Task<LiveMatchModel> GetLiveMatchByTeam(string teamId);
        Task<List<string>> GetExistingMatchesId();
        Task<bool> MatchExists(string matchId);
        Task<bool> UpdateLiveMatch(LiveMatchModel match);
        Task<bool> AddLiveMatch(LiveMatchModel match);
        Task<List<LiveMatchModel>> GetMatchesByStatus(List<int> status);
        Task<bool> AddToRecentMatch(RecentMatchModel match);
        Task<bool> UpdateRecentMatch(RecentMatchModel match);
        Task<bool> DeleteLiveMatchById(string matchId);
        Task<Team> GetTeamById(string teamId);
        Task<Competition> GetCompetitionById(string teamId);
        Task<List<LiveMatchModel>> SearchLiveMatchesByQuery(string query);
        Task<List<UpcomingMatch>> SearchUpcomingMatchesByQuery(string query);
        Task<List<RecentMatchModel>> SearchRecentMatchesByQuery(string query);
        Task<bool> UpdateLiveMatchesRange(List<LiveMatchModel> matches);

        Task<List<string>> SearchTeamName(string query);
        Task<List<RecentMatchModel>> GetMatchesById(List<string> teamIds);

        Task<List<RecentMatchModel>> SearchRecentMatches(string query);

    }
}
