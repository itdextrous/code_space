using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IFeatureCounterService
    {
        Task<Result<bool>> HitLiveInsightPerGame(string userId, string matchId);

    }
}
