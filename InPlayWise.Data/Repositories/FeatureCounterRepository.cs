using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;

namespace InPlayWise.Data.Repositories
{
    public class FeatureCounterRepository : IFeatureCounterRepository
    {

        private readonly AppDbContext _db;

        public FeatureCounterRepository(AppDbContext dbContext) {
            _db = dbContext;
        }

        public async Task<bool> AddUserHitToLiveInsightPerGame()
        {
            try
            {
                return false;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return true;
            }
        }

        public async Task<bool> AddUserHitToLiveInsightPerGame(string userId, string matchId)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return true;
            }
        }

        public async Task<bool> CreateFeatureCounter(FeatureCounter counter)
        {
            try
            {
                //await _db.FeatureCounters.AddAsync(counter);
                //await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> FeatureCounterExists(string userId)
        {
            try
            {
                //return (await _db.FeatureCounters.FindAsync(userId)) is not null;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }



        public async Task<bool> LiveInsightsPerGameCounterExists(string userId, string matchId)
        {
            try
            {
                //LiveInsightsPerGame counters = await _db.LiveInsightsPerGameCounters.FindAsync(userId);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}
