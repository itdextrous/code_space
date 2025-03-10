using InPlayWise.Data.Entities.FeaturesCountEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IFeatureCounterRepository
    {
        Task<bool> AddUserHitToLiveInsightPerGame(string userId, string matchId);

        Task<bool> FeatureCounterExists(string userId);
        Task<bool> CreateFeatureCounter(FeatureCounter counter);


        Task<bool> LiveInsightsPerGameCounterExists(string userId, string matchId);
    }
}
