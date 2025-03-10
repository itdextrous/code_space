using InPlayWise.Common.DTO;
using InPlayWise.Data.DTO;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface ILiveMatchService
    {
        Task<Result<bool>> BroadcastMatches();
        Result<List<LiveMatchDto>> GetAllLiveMatches(); 
        Task<LiveMatchModel> GetLiveMatch(string matchId);
        Task<Result<LiveMatchModel>> LiveMatchInfo(string matchId);

        Task<Result<List<LiveMatchModel>>> GetCompleteLiveMatches();

        Task<Result<List<LiveMatchBasicDto>>> AllLiveMatchesBasicInfo();

        Task<Result<LiveMatchBasicDto>> GetLiveMatchBasicInfo(string MatchId);

        Task<Result<SearchGamesResponseDto>> SearchMatches(string query);

        Task<Result<List<LiveMatchFilterDto>>> AllLiveMatchesFilter();

    }
}
