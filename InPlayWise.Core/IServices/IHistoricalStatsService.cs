using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IHistoricalStatsService
    {

        Task<List<RecentMatchModel>> GetMatches(string teamId);
        Task<Result<HistoricalStatsResponseMatch>> GetStatsOfMatch(string matchId);
        Task<HistoricalStatsResponseTeam> GetTeamStats(string teamId);

	}
}
