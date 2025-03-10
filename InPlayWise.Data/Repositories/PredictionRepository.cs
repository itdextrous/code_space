//using InPlayWise.Common.DTO;
//using InPlayWise.Data.Entities;
//using InPlayWise.Data.Entities.PredictionEntities;
//using InPlayWise.Data.IRepositories;
//using InPlayWise.Data.SportsEntities;
//using InPlayWiseData.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace InPlayWise.Data.Repositories
//{
//    public class PredictionRepository : IPredictionRespository
//	{

//		private readonly AppDbContext _db;
//		private readonly ILogger<PredictionRepository> _logger; 
//		public PredictionRepository(AppDbContext appDb, ILogger<PredictionRepository> logger)
//		{
//			_db = appDb;
//			_logger = logger;
//		}

//        public async Task<List<PredictionModel>> GetAllData()
//        {
//			try
//			{
//				return await _db.PredictionData.ToListAsync();
//			}catch(Exception ex)
//			{
//				Console.WriteLine(ex);
//				return new List<PredictionModel>();
//			}
//        }

//        public async Task<List<PredictionModel>> GetMatchData(string matchId)
//		{
//			try
//			{
//				return await _db.PredictionData.Where(pd => pd.MatchId == matchId).ToListAsync();
				
//			}catch(Exception ex)
//			{
//				Console.WriteLine(ex);
//				return new List<PredictionModel>();
//			}
//		}

//        public async Task<List<LiveMatchModel>> GetAllLiveMatches()
//        {
//            try
//            {
//				return await _db.LiveMatches.ToListAsync();
//            }
//            catch (Exception ex)
//            {
//				_logger.LogError(ex.ToString());
//                return null;
//            }
//        }

//        public async Task<bool> UploadRange(List<PredictionModel> dataList)
//		{
//			try
//			{
//				await _db.PredictionData.AddRangeAsync(dataList);
//				await _db.SaveChangesAsync();
//				return true;
//			}catch(Exception ex)
//			{
//				Console.WriteLine(ex);
//				return false;
//			}
//		}

//        public async Task<LiveMatchModel> GetLiveMatchById(string matchId)
//        {
//            try
//            {
//				return await _db.LiveMatches.FindAsync(matchId);
//            }
//            catch (Exception ex)
//            {
//				_logger.LogError(ex.ToString());
//                return null;
//            }
//        }

//        public async Task<bool> AddDailyPredictionDataList(List<DailyPrediction> data)
//        {
//            try
//            {
//				await _db.DailyPredictionData.AddRangeAsync(data);
//				await _db.SaveChangesAsync();
//				return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return false;
//            }
//        }

//        public async Task<List<DailyPrediction>> GetDailyPredictionData()
//        {
//            try
//            {
//                return await _db.DailyPredictionData.ToListAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return null ;
//            }
//        }

//        public async Task<bool> DeleteDailyPrediction()
//        {
//            try
//            {
//                List<DailyPrediction> all = await _db.DailyPredictionData.ToListAsync();
//                _db.DailyPredictionData.RemoveRange(all);
//                await _db.SaveChangesAsync();
//                return true;
//            }catch(Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return false;
//            }
//        }

//        public async Task<bool> AddToAllPredictions(List<AllPrediction> data)
//        {
//            try
//            {
//                await _db.AllPredictionData.AddRangeAsync(data);
//                await _db.SaveChangesAsync();
//                return true;
//            }catch(Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return false;
//            }
//        }

//        //public async Task<List<DailyPredictionDataset>> GetDailyPredictionDataset()
//        //{
//        //    try
//        //    {
//        //        return await _db.DailyPredictionDataset.ToListAsync();
//        //    }catch(Exception ex)
//        //    {
//        //        _logger.LogError(ex.ToString());
//        //        return null;
//        //    }
//        //}

//		//public async Task<bool> AddToDailyPredictionDataset2(List<DailyPredictionDataset> data)
//		//{
//  //          try
//  //          {
//  //              await _db.DailyPredictionDataset.AddRangeAsync(data);
//  //              await _db.SaveChangesAsync();
//  //              return true;
//  //          } catch (Exception ex)
//  //          {
//  //              _logger.LogError(ex.ToString());
//  //              return false;
//  //          }
//		//}

//		//public async Task<bool> AddToAllPredictionDataset2(List<AllPredictionDataset> data)
//		//{
//		//	try
//		//	{
//		//		await _db.AllPredictionDataset.AddRangeAsync(data);
//		//		await _db.SaveChangesAsync();
//		//		return true;
//		//	}
//		//	catch (Exception ex)
//		//	{
//		//		_logger.LogError(ex.ToString());
//		//		return false;
//		//	}
//		//}

//		//public async Task<bool> DeleteFromDailyPredictionDataset()
//		//{
//  //          try
//  //          {
//  //              List<DailyPredictionDataset> dt = await _db.DailyPredictionDataset.ToListAsync();
//  //              _db.DailyPredictionDataset.RemoveRange(dt);
//  //              await _db.SaveChangesAsync();
//  //              return true;
//  //          }catch(Exception ex)
//  //          {
//  //              _logger.LogError(ex.ToString());
//  //              return false;
//  //          }
//		//}
//	}
//}
