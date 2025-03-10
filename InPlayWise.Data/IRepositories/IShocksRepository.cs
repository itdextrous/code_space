using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IShocksRepository
    {
        Task<int> AddCounterIfDoesntExistsAndReturnHits(ShockCounter counter);
        Task<PlanFeatures> GetPlanFeatures(string userId);

    }
}
