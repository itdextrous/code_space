using InPlayWise.Data.DTO;
using InPlayWise.Data.Entities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
	public interface IInsightService
    {
        Task<Result<Insights>> GetAllInsightsOfTeam(string teamId);
        //Task<Result<MatchInsightsResponseDto>> GetInsightsOfMatch(string matchId);
    }
}