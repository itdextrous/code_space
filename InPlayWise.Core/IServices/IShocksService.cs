using InPlayWise.Data.DTO;
using InPlayWise.Data.Entities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IShocksService
    {
        public Task<Result<MatchShocksResponseDto>> GetShockFactsOfMatch(string matchId);

    }
}
