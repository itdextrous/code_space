using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.Prediction;
using InPlayWise.Data.Entities.PredictionEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InPlayWise.Data.Repositories
{
    public class OpportunitiesPredictionRepository : IOpportunitiesPredictionRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<OpportunitiesPredictionRepository> _logger;
        public OpportunitiesPredictionRepository(AppDbContext context, ILogger<OpportunitiesPredictionRepository> logger) {
            _db = context;
            _logger = logger;
        }

        public async Task<bool> DeleteAndRefillOpportunities(List<Opportunity> opportunities)
        {
            try
            {
                //List<Opportunity> AllOpportunities = await _db.Opportunities.ToListAsync();
                //_db.Opportunities.RemoveRange(AllOpportunities);
                //await _db.Opportunities.AddRangeAsync(opportunities);
                //await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<List<LiveMatchModel>> GetAllLiveMatches()
        {
            try
            {
                return (await _db.LiveMatches.ToListAsync());
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<List<Opportunity>> GetAllOpportunities()
        {
            try
            {
                //return (await _db.Opportunities.OrderByDescending(op => op.Match.MatchStartTimeOfficial).ToListAsync());
                return new List<Opportunity>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<List<Opportunity>> GetOpportunitiesByMatchId(string matchId)
        {
            try
            {
                //return (await _db.Opportunities.Where(op => op.MatchId.Equals(matchId)).ToListAsync());
                return new List<Opportunity>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }


        public async Task<PlanFeatures> GetPlanFeatures(string userId)
        {
            try
            {
                Guid productId = (await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(userId))).ProductId;
                PlanFeatures feature = await _db.PlanFeatures.SingleOrDefaultAsync(ft => ft.ProductId.Equals(productId));
                return feature;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<PredictionCounter>> GetUserHitsOnMatches(string userId)
        {
            try
            {
                return await _db.PredictionCounters.Where(pc => pc.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return null; }
        }

        public async Task<bool> AddCounter(PredictionCounter counter)
        {
            try
            {
                await _db.PredictionCounters.AddAsync(counter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return false; }
        }

        public async Task<bool> UpdateCounter(PredictionCounter counter)
        {
            try
            {
                PredictionCounter exCounter = await _db.PredictionCounters.FindAsync(counter.Id);
                _db.Entry(exCounter).CurrentValues.SetValues(counter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return false; }
        }


        //public async Task<List<DailyPredictionDataset>> GetDailyPredictionDataset()
        //{
        //    try
        //    {
        //        return await _db.DailyPredictionDataset.Take(10000).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        return null;
        //    }
        //}




        //public async Task<bool> AddToDailyPredictionDataset(List<DailyPredictionDataset> data)
        //{
        //    try
        //    {
        //        await _db.DailyPredictionDataset.AddRangeAsync(data);
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        return false;
        //    }
        //}

        //public async Task<bool> AddToAllPredictionDataset(List<AllPredictionDataset> data)
        //{
        //    try
        //    {
        //        await _db.AllPredictionDataset.AddRangeAsync(data);
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        return false;
        //    }
        //}

        //public async Task<bool> DeleteFromDailyPredictionDataset()
        //{
        //    try
        //    {
        //        List<DailyPredictionDataset> dt = await _db.DailyPredictionDataset.ToListAsync();
        //        _db.DailyPredictionDataset.RemoveRange(dt);
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        return false;
        //    }
        //}

        //public async Task<List<AllPredictionDataset>> GetAllPredictionDatasets(int n)
        //{
        //    try
        //    {
        //        return await _db.AllPredictionDataset.Take(n).ToListAsync();
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        return null;
        //    }
        //}

        public async Task<bool> SavePredictionRecords(List<PredictionRecord> preds)
        {
            try
            {
                await _db.PredictionRecords.AddRangeAsync(preds);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<List<PredictionRecord>> GetRecordByMatchId(string matchId)
        {
            try
            {
                return await _db.PredictionRecords.Where(pred => pred.MatchId.Equals(matchId)).OrderBy(pr => pr.MatchMinute).ThenBy(pr => pr.OpportunityName).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<string>> GetMatchesIdWithRecords()
        {
            try
            {
                return await _db.PredictionRecords.Select(pr => pr.MatchId).Distinct().Take(15).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

		//public async Task<bool> UploadToActiveMatchesPrediction(List<PredictionActiveMatchesData> dataset)
		//{
  //          try
  //          {
  //              await _db.PredictionActiveMatchesDataset.AddRangeAsync(dataset);
  //              await _db.SaveChangesAsync();
  //              return true;
  //          }
  //          catch(Exception ex)
  //          {
  //              _logger.LogError(ex.ToString());
  //              return false;
  //          }
		//}

		//public async Task<List<PredictionActiveMatchesData>> GetPredictionDatasetForActiveMatch(string matchId)
		//{
		//	try
		//	{
		//		return await _db.PredictionActiveMatchesDataset.Where(d => d.MatchId.Equals(matchId)).ToListAsync();
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.ToString());
		//		return null;
		//	}
		//}

		//public async Task<bool> UploadToFullPredictionData(List<PredictionFullData> dataset)
		//{
		//	try
		//	{
		//		await _db.PredictionFullDataSet.AddRangeAsync(dataset);
		//		await _db.SaveChangesAsync();
		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.ToString());
		//		return false;
		//	}
		//}

        public async  Task<float> GetAccuracyByMatchIdAsync(string matchId, string opportunity)
        {
            try
            {
                var accuracy = await _db.PredictionAccuracies
            .Where(pa => pa.MatchId == matchId && pa.Opportunity == opportunity)
            .Select(pa => pa.Accuracy)
            .FirstOrDefaultAsync();
                return accuracy;// Will be 0.0f if no match is found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<float> GetAccuracyByDate(DateTime date, string opportunity)
        {
            try
            {
                var averageAccuracy = await _db.PredictionAccuracies
        .Where(pa => pa.EndTime > date && pa.Opportunity == opportunity)
        .AverageAsync(pa => pa.Accuracy);

                return averageAccuracy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<List<PredictionAccuracy>> GetMatchListAccuracy(DateTime date)
        {
            try
            {
                var predictions = await _db.PredictionAccuracies
           .Where(pa => pa.EndTime > date)
           .ToListAsync();

                return predictions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async  Task<List<PredictionAccuracy>> GetAllAccuracies()
        {
            try
            {
                var predictions = await _db.PredictionAccuracies.ToListAsync();
                return predictions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<bool> SavePredictionAccuracies(List<PredictionAccuracy> predictionAccuracies)
        {
            try
            {
                await _db.PredictionAccuracies.AddRangeAsync(predictionAccuracies);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeletePredictionRecord(string matchId)
        {
            try
            {
                var records = _db.PredictionRecords.Where(pr => pr.MatchId == matchId);

                _db.PredictionRecords.RemoveRange(records);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return false;
            }
        }
    }
}
