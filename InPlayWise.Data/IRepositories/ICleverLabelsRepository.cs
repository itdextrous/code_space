using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface ICleverLabelsRepository
    {

        Task<List<RecentMatchModel>> GetLastThreeMatchesOfTeam(string teamId);
        Task<PlanFeatures> GetPlanFeatures(string userId);
        Task<List<CleverLabelsCounter>> GetUserHitsOnMatches(string userId);
        Task<bool> AddCounter(CleverLabelsCounter counter);
        Task<bool> UpdateCounter(PredictionCounter counter);
        Task<List<LiveMatchModel>> GetAllLiveMatches();
        Task<List<RecentMatchModel>> GetAllMatchesOfSeason(string seasonId);
        Task<List<RecentMatchModel>> GetPastMatchesOfTeam(string teamId);


    }
}
