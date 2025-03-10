using InPlayWise.Common.Constants;
using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IProfileRepository
    {

        Task<bool> CreateProfile(string userId);
        Task<bool> ThemeDark(string userId);
        Task<bool> SetDarkTheme(string userId);
        Task<bool> SetLightTheme(string userId);
        Task<bool> AddFavouriteTeam(string userId, List<string> teamIds);
        Task<bool> AddFavouriteCompetition(string userId, List<string> leagueIds);
        Task<List<Team>> GetFavouriteTeams(string userId);
        Task<List<string>> GetFavouriteTeamIds(string userId);
        Task<List<Competition>> GetFavouriteCompetitions(string userId);

        Task<bool> DeleteFavouriteCompetitions(string userId, List<string> competitionIds);

        Task<bool> DeleteFavouriteTeams(string userId, List<string> teamIds);




        // These are the user quota repos 
        Task<PlanFeatures> GetPlanFeatures(string userId);
        Task<List<PredictionCounter>> GetPredictionRequestsOfUser(string userId);
        Task<int> AccumulatorGeneratorCounterOfUser(string userId);
        Task<List<ShockCounter>> GetShockCounterRequestOfUser(string userId);
        Task<List<CleverLabelsCounter>> GetCleverLabelsRequestOfUser(string userId);
        Task<int> GetHistoryOfAccumulatorsRequestOfUser(string userId);
        Task<int> GetHistoryOfWiseProHedgeOfUser(string userId);
        Task<List<LeagueStatsCount>> GetLeagueStatsRequestOfUser(string userId);


        //Task<bool> SetCountry(int index);







    }
}
