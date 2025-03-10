using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;

namespace InPlayWise.Data.Repositories
{
    public class UpcomingMatchesRepository : IUpcomingMatchesRepository
    {

        private readonly AppDbContext _db;

        public UpcomingMatchesRepository(AppDbContext db) {
            _db = db;
        }

        public async Task<bool> DeleteMatches(List<UpcomingMatch> matchesToDelete)
        {
            try
            {
                List<UpcomingMatch> matches = await _db.UpcomingMatches.ToListAsync();
                _db.UpcomingMatches.RemoveRange(matchesToDelete);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<UpcomingMatch>> GetAllUpcomingMatches()
        {
            try
            {
                
                return await _db.UpcomingMatches.Include(um => um.HomeTeam)
                    .Include(um => um.AwayTeam).Include(um => um.Competition).OrderBy(um => um.time)
                    .ToListAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<UpcomingMatch>> SearchMatchByQuery(string query)
        {
            try
            {
                return await _db.UpcomingMatches.Where(um => um.HomeTeam.Name.Contains(query) || um.AwayTeam.Name.Contains(query)).Include(um => um.HomeTeam)
                    .Include(um => um.AwayTeam).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
