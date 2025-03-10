using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IInsightsRepository
    {


        Task<List<LiveMatchModel>> GetAllLiveMatches();

        Task<List<string>> GetAllInsightsMatchId();
        Task<List<RecentMatchModel>> GetMatchesByNum(string teamId, int n = 1);

        Task<List<RecentMatchModel>> GetLastNMatchesOfTeamAsHome(string teamId, int n = 1);

        Task<List<RecentMatchModel>> GetMatchesByTime(string teamId, int time = 0);
        Task<List<RecentMatchModel>> GetMatchesBySeason(string teamId, string seasonId = "");
        Task<List<RecentMatchModel>> GetLastNMatchesOfTeamAsAway(string teamId, int n = 1);
        Task<List<RecentMatchModel>> GetLastNMatchesOfTeam(string teamId, int n = 1);
        Task<Insights> GetInsightsOfTeam(string teamId);
        Task<bool> AddInsights(Insights insights);
        Task<bool> UpdateInsights(Insights insights);
        Task<List<Insights>> GetInsightsOfMatch(string matchId);
        Task<LiveMatchModel> GetLiveMatchById(string matchId);
        Task<bool> AddHit(LiveInsightsPerGame counter);
        Task<List<Insights>> GetAllInsights();
        Task<bool> RemoveAndUpdateAllInsights(List<Insights> insights);
		Task<PlanFeatures> GetPlanFeatures(string userId);
        Task<LiveInsightsPerGame> GetUserHitsOnMatch(string userId, string matchId);
        Task<bool> AddCounter(LiveInsightsPerGame counter);
        Task<bool> UpdateCounter(LiveInsightsPerGame counter);
	}
}
