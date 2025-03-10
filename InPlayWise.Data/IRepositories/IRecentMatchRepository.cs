using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IRecentMatchRepository
    {

		Task<RecentMatchModel> GetMatchById(string matchId);

        Task<bool> UpdateRecentMatch(RecentMatchModel match);

		Task<List<RecentMatchModel>> GetMatchesOfTeamById(string teamId);
        Task<List<RecentMatchModel>> GetLast50Matches();
        
        Task<List<RecentMatchModel>> GetMatchesByCompetition(string competitionId);
        Task<List<RecentMatchModel>> GetLastNMatchesOfTeam(string teamId, int n);

        Task<List<RecentMatchModel>> SearchRecentMatchesByQuery(string query);


        Task<List<RecentMatchModel>> GetAllMatches();
        Task<bool> UpdateAllMatches(List<RecentMatchModel> matches);

        Task<List<RecentMatchModel>> GetThirtyDaysCompletedMatches();

        Task<List<RecentMatchModel>> GetIncompleteMatches();

        Task<bool> UpdateIncompleteMatches(List<RecentMatchModel> matches);

        Task<List<string>> GetAllRecentMatchesId();


        Task<bool> SaveRecentMatches(List<RecentMatchModel> matches);

        Task<List<RecentMatchModel>> GetMatchesWithoutName();

        Task<List<Team>> GetAllTeams();

        Task<List<Competition>> GetAllCompetitions();


        Task<bool> DeleteFutureMatches();

        Task<List<RecentMatchModel>> LocalDbGetAllMatches();
        Task<bool> LocalDbUploadMatches(List<RecentMatchModel> matches);

        Task<bool> DeleteMatches(List<string> ids);

        Task<List<RecentMatchModel>> GetMatchesWithId(List<string> ids);

        Task<bool> AddMatches(List<RecentMatchModel> matches); 

	}
}
