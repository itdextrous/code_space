using InPlayWise.Common.DTO;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IRecentMatchService
    {
        Task<Result<List<RecentMatchDto>>> MatchesOfTeamById(string teamId);
        Task<Result<List<RecentMatchDto>>> Last50Matches();
        Task<Result<RecentMatchDto>> MatchById(string matchId);
        Task<Result<List<RecentMatchDto>>> MatchesByCompetition(string competitionId);
        Task<Result<List<RecentMatchDto>>> GetLastThreeMatchesOfTeam(string teamId);
        Task<Result<List<RecentMatchDto>>> GetLastNMatchesOfTeam(string teamId, int n);
        Task<List<RecentMatchDto>> GetMatchesOfTeamFromDbByTeamid(string teamId);
        //Task<Result<List<RecentMatchDto>>> SearchRecentMatches(string query);
        Task<Result<RecentMatchDto>> GetMatchFromDb(string matchId);
        Task<Result<bool>> GetHistoricalMatchesOnLocal();
        Task<Result<object>> TestPoint();

	}
}
