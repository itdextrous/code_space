using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IUpcomingMatchesRepository
    {
        Task<List<UpcomingMatch>> GetAllUpcomingMatches();
        Task<List<UpcomingMatch>> SearchMatchByQuery(string query);
        Task<bool> DeleteMatches(List<UpcomingMatch> matches);
    }
}
