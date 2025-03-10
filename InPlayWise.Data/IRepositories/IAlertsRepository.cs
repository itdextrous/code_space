using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IAlertsRepository
    {

        Task<bool> AddAlert(MatchAlert alert);
        Task<UpcomingMatch> GetMatch(string matchId);
        Task<bool> AlertExists(MatchAlert alert);
        Task<List<MatchAlert>> GetAllAlerts();

        Task<List<MatchAlert>> GetTrigerredAlerts(); 

        Task<bool> AddOrUpdateAlert(MatchAlert alert);

        Task<List<UpcomingMatch>> GetUpcomingMatches();

        Task<bool> DeleteMultipleAlerts(List<MatchAlert> alerts);

    }
}
