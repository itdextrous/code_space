using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface ILeagueStatsRepository
    {
        Task<List<LeagueStats>> GetTenLeagueStats();
        Task<LeagueStats> GetLeagueStatsByLeagueId(string  leagueId);
        Task<bool> UpdateLeagueStats(LeagueStats leagueStats);
        Task<bool> AddLeagueStats(LeagueStats leagueStats);
        Task<bool> DeleteLeagueStats(string leagueId);

        Task<PlanFeatures> GetPlanFeatures(string userId);
        Task<List<LeagueStatsCount>> GetHitsRecordOfUser(string userId);
        Task<bool> AddLeagueStatsCounter(LeagueStatsCount counter);
        Task<List<RecentMatchModel>> GetAllRecentMatchesOfSeason(string seasonId);
        Task<Competition> GetCompetition(string id);

        Task<bool> SetFavouriteLeagueStats(List<FavouriteLeagueStat> leagueIds);
        Task<List<FavouriteLeagueStat>> GetFavouriteLeagueStats(string userId);

    }
}
