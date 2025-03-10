using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.PredictionEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
	public interface IPredictionRespository
	{
		Task<bool> UploadRange(List<PredictionModel> data);
		Task<List<PredictionModel>> GetMatchData(string matchId);
		Task<List<PredictionModel>> GetAllData();
		Task<List<LiveMatchModel>> GetAllLiveMatches();
		Task<LiveMatchModel> GetLiveMatchById(string matchId);
		Task<bool> AddDailyPredictionDataList(List<DailyPrediction> data);
		Task<List<DailyPrediction>> GetDailyPredictionData();
		Task<bool> DeleteDailyPrediction();
		Task<bool> AddToAllPredictions(List<AllPrediction> data);



		Task<List<DailyPredictionDataset>> GetDailyPredictionDataset();
		Task<bool> AddToDailyPredictionDataset2(List<DailyPredictionDataset> data);
		Task<bool> AddToAllPredictionDataset2(List<AllPredictionDataset> data);
		Task<bool> DeleteFromDailyPredictionDataset();

	}
}
