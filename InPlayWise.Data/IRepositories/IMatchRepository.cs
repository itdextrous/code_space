using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Data.IRepositories
{
    public interface IMatchRepository
	{
		Task<Result<List<RecentMatchModel>>> GetRecentMatchesOfTeam(string teamId);
		Task<Result<List<RecentMatchModel>>> GetHomeMatchesOfTeam(string teamId);
		Task<Result<List<RecentMatchModel>>> GetAwayMatchesOfTeam(string teamId);
		Task<Result<List<RecentMatchModel>>> GetSeasonGamesOfTeam(string teamId, string seasonId);
		Task<bool> UploadLiveMatch(LiveMatchModel match);
		Task<bool> UpdateLiveMatch(LiveMatchModel match);

		Task<bool> UploadRecentMatch(RecentMatchModel match);
		Task<bool> UpdateRecentMatch(RecentMatchModel match);
        Task<bool> UploadLiveMatches(List<LiveMatchModel> liveMatches);
		Task<List<string>> GetHomeAndAwayTeamOfMatch(string matchId);
		Task<bool> UploadMatches(List<RecentMatchModel> matches);
		Task<RecentMatchModel> GetLiveMatches(string teamId);
		Task<List<LiveMatchModel>> GetAllLiveMatches();
		Task<bool> DeleteEndedMatch();
		Task<LiveMatchModel> GetLiveMatch(string matchId);
		Task<List<LiveMatchModel>> GetEndedMatches();
		Task<bool> DeleteLiveMatch(string matchId);
		Task<RecentMatchModel> LiveMatchInfo(string matchId);
		Task<List<LiveMatchModel>> GetCompleteLiveMatches();

    }
}
