using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IUpcomingMatchesService
    {
        Task<Result<List<UpcomingMatch>>> GetAllUpcomingMatches();

        Task<Result<List<UpcomingMatch>>> SearchUpcomingMatches(string query);


        Task<Result<bool>> DeleteOldMatches();

        Task<bool> SeedUpcomingMatchesInMemory();



    }
}
