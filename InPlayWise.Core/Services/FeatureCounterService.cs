using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.Services
{
    public class FeatureCounterService : IFeatureCounterService
    {

        private readonly IFeatureCounterRepository _featuresRepo ;

        public FeatureCounterService(IFeatureCounterRepository featureCounterRepository)
        {
            _featuresRepo = featureCounterRepository;
        }

        public async Task<Result<bool>> HitLiveInsightPerGame(string userId, string matchId)
        {
            try
            {
                bool exists = await _featuresRepo.LiveInsightsPerGameCounterExists(userId, matchId);
                
                LiveInsightsPerGame liveInsights = new LiveInsightsPerGame()
                {
                    //Id = Guid.NewGuid(),
                    //FeatureCounterId = userId,
                    MatchId = matchId,
                    Hits = 0
                };

                

                return new Result<bool>(200, true, "API hit successful", false);
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>(500, false, "Internal Server Error", false) ;
            }
        }


        private async Task<bool> CreateFeaturesCounterIfDoesntExists(string userId)
        {
            try
            {
                bool exists = await _featuresRepo.FeatureCounterExists(userId);
                if (exists) return true;
                FeatureCounter counter = new FeatureCounter()
                {
                    UserId = userId
                };
                bool added = await _featuresRepo.CreateFeatureCounter(counter);
                return added is true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }



        private async Task<bool> CreateLiveInsightsPerGameCounterIfDoesntExists(string userId, string matchId)
        {
            try
            {
                bool exists = await _featuresRepo.FeatureCounterExists(userId);
                if (exists) return true;
                FeatureCounter counter = new FeatureCounter()
                {
                    UserId = userId
                };
                bool added = await _featuresRepo.CreateFeatureCounter(counter);
                return added is true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }



    }
}
