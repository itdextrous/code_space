using InPlayWise.Common.DTO.FootballResponseModels.OddsDataResponseModels;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IOddsDataService
    {
        Task<Result<RealTimeOddsResponseDto>> GetRealTimeOdds();
        Task<Result<SingleMatchOddsResponseDto>> GetSingleMatchOdds(string singleMatchOdds);
    }
}
