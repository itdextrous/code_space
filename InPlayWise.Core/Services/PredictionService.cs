//using InPlayWise.Core.InMemoryServices;
//using InPlayWise.Core.IServices;
//using InPlayWise.Data.DTO;
//using InPlayWise.Data.DTO.OpportunitiesEntities;
//using InPlayWise.Data.Entities.PredictionEntities;
//using InPlayWise.Data.IRepositories;
//using InPlayWise.Data.SportsEntities;
//using InPlayWiseCommon.Wrappers;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Text;
//using System.Text.Json;

//namespace InPlayWiseCore.Services
//{
//	public class PredictionService : IPredictionServices
//    {
//        private readonly IPredictionRespository _predRepo;
//        private readonly ILogger<PredictionService> _logger;
//        private readonly IConfiguration _config;
//        private readonly string _baseUrl ;

//        private readonly MatchInMemoryService _inMemory;

//        public PredictionService(IPredictionRespository predRepo, ILogger<PredictionService> logger, IConfiguration config, MatchInMemoryService mServ) {
//            _predRepo = predRepo;
//            _logger = logger;
//            _config = config;
//            _inMemory = mServ;
//            _baseUrl = _config.GetSection("Prediction:LocalBaseUrl").Value;
//        }


//        //public async Task<short> PredictGoal(string matchId)
//        //{
//        //    try
//        //    {
//        //        LiveMatchModel match = await _predRepo.GetLiveMatchById(matchId);
//        //        if(!(match.MatchStatus == 2 || match.MatchStatus == 4))
//        //        {
//        //            return 0;
//        //        }
//        //        LiveMatchModel matchInfo = await _predRepo.GetLiveMatchById(matchId);
//        //        List<Features> features = MapLiveMatchToFeatures(matchInfo);
//        //        ApiTrainPayload data = new ApiTrainPayload();
//        //        data.items = features;
//        //        HttpClient client = new HttpClient();
//        //        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
//        //        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//        //        var response = await client.PostAsync(
//        //               "https://inplaywise-data.azurewebsites.net//predict_next_goal", content);
//        //        var result = await response.Content.ReadAsStringAsync();
//        //        JsonDocument document = JsonDocument.Parse(result);
//        //        JsonElement root = document.RootElement;
//        //        string results = root.GetProperty("chances").ToString();
//        //        short probNum = (short)float.Parse(results);
//        //        return probNum;
//        //    }
//        //    catch(Exception ex)
//        //    {
//        //        _logger.LogError(ex.Message);
//        //        return 0 ;
//        //    }
//        //}

//        //public async Task<bool> TrainModel()
//        //{
//        //    try
//        //    {
//        //        List<PredictionModel> inputData = await _predRepo.GetAllData();
//        //        List<Features> features = MapPredictionsToFeatures(inputData);
//        //        ApiTrainPayload apiTrain = new ApiTrainPayload();
//        //        apiTrain.items = features;
//        //        HttpClient client = new HttpClient();
//        //        var data = apiTrain;
//        //        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
//        //        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//        //        var response = await client.PostAsync(
//        //               "https://inplaywise-data.azurewebsites.net//train_and_save_model", content );
//        //        var result = await response.Content.ReadAsStringAsync();
//        //        Console.WriteLine(response);
//        //        return true;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError(ex.Message);
//        //        return false;
//        //    }
//        //}

//        //internal class ApiTrainPayload
//        //{
//        //    public List<Features> items { get; set; }
//        //}

//        //internal List<Features> MapPredictionsToFeatures(List<PredictionModel> predictions)
//        //{
//        //    List<Features> featuresList = new List<Features>();
//        //    foreach(var prediction in predictions)
//        //    {
//        //        Features features = new Features();
//        //        features.matchId = prediction.MatchId;
//        //        features.matchStatus = prediction.MatchStatus;
//        //        features.matchMinutes = prediction.MatchMinutes;
//        //        features.extraTime = prediction.ExtraTime;
//        //        features.homeGoals = prediction.HomeGoals;
//        //        features.awayGoals = prediction.AwayGoals;
//        //        features.homeRed = prediction.HomeRed;
//        //        features.awayRed = prediction.AwayRed;
//        //        features.homeYellow = prediction.HomeYellow;
//        //        features.awayYellow = prediction.AwayYellow;
//        //        features.homeCorners = prediction.HomeCorners;
//        //        features.awayCorners = prediction.AwayCorners;
//        //        features.homeShotsOffTarget = prediction.HomeShotsOffTarget;
//        //        features.awayShotsOffTarget = prediction.AwayShotsOffTarget;
//        //        features.homeShotsOnTarget = prediction.HomeShotsOnTarget;
//        //        features.awayShotsOnTarget = prediction.AwayShotsOnTarget;
//        //        features.homeDangerousAttacks = prediction.HomeDangerousAttacks;
//        //        features.awayDangerousAttacks = prediction.AwayDangerousAttacks;
//        //        features.homeAttacks = prediction.HomeAttacks;
//        //        features.awayAttacks = prediction.AwayAttacks;
//        //        features.homePenalties = prediction.HomePenalties;
//        //        features.awayPenalties = prediction.AwayPenalties;
//        //        features.homePossession = prediction.HomePossession;
//        //        features.awayPossession = prediction.AwayPossession;
//        //        features.homeOwnGoals = prediction.HomeOwnGoals;
//        //        features.awayOwnGoals = prediction.AwayOwnGoals;
//        //        featuresList.Add(features);
//        //    }
//        //    return featuresList;
//        //}


//        //internal List<Features> MapLiveMatchToFeatures(LiveMatchModel liveMatch)
//        //{
//        //    Features features = new Features()
//        //    {
//        //        matchId = liveMatch.MatchId,
//        //        matchStatus = liveMatch.MatchStatus,
//        //        matchMinutes = liveMatch.MatchMinutes,
//        //        homeGoals = liveMatch.HomeGoals,
//        //        awayGoals = liveMatch.AwayGoals,
//        //        homeRed = liveMatch.HomeRed,
//        //        awayRed = liveMatch.HomeRed,
//        //        homeYellow = liveMatch.HomeYellow,
//        //        awayYellow = liveMatch.AwayYellow,
//        //        homeCorners = liveMatch.HomeCorners,
//        //        awayCorners = liveMatch.AwayCorners,
//        //        homeShotsOffTarget = liveMatch.HomeShotsOffTarget,
//        //        awayShotsOffTarget = liveMatch.AwayShotsOffTarget,
//        //        homeShotsOnTarget = liveMatch.HomeShotsOnTarget,
//        //        awayShotsOnTarget = liveMatch.AwayShotsOnTarget,
//        //        homeDangerousAttacks = liveMatch.HomeDangerousAttacks,
//        //        awayDangerousAttacks = liveMatch.AwayDangerousAttacks,
//        //        homeAttacks = liveMatch.HomeAttacks,
//        //        awayAttacks = liveMatch.AwayAttacks,
//        //        homePenalties = liveMatch.HomePenalties,
//        //        awayPenalties = liveMatch.AwayPenalties,
//        //        homePossession = liveMatch.HomePossession,
//        //        awayPossession = liveMatch.AwayPossession,
//        //        homeOwnGoals = liveMatch.HomeOwnGoals,
//        //        awayOwnGoals = liveMatch.AwayOwnGoals
//        //    };
//        //    List<Features> featuresList = new List<Features>();
//        //    featuresList.Add(features);
//        //    return featuresList;
//        //}



//        //*******************************************************************
//        //*******************************************************************
//        //*******************************************************************


//        //public async Task<Result<bool>> TrainAll()
//        //{
//        //    try
//        //    {
//        //        List<string> trainingUrls = PrepareTrainingUrls();
//        //        List<DailyPrediction> data = await _predRepo.GetDailyPredictionData();
//        //        string res = "";
//        //        foreach(string url in trainingUrls)
//        //        {
//        //            res = await HitTrainingUrl(url, data);
//        //        }
//        //        bool suc = res.Equals("trained successfully");
//        //        return new Result<bool>(suc ? 200 : 500, suc, suc ? "trained" : "failed", suc);
//        //    }catch(Exception ex)
//        //    {
//        //        _logger.LogError(ex.ToString());
//        //        return new Result<bool>(500, false, "Internal Server error", false);
//        //    }
//        //}

//        public async Task<bool> UploadDailyPredictionDataset()
//        {
//            try
//            {
//                List<LiveMatchModel> matches = await _predRepo.GetAllLiveMatches();
//                if(matches is null) return false;
//                List<DailyPredictionDataset> dataSet = new List<DailyPredictionDataset>();
//                foreach(LiveMatchModel match in matches)
//                    dataSet.Add(new DailyPredictionDataset(match));
//                bool uploaded = await _predRepo.AddToDailyPredictionDataset2(dataSet);
//                return uploaded;
//            }catch(Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return false;
//            }
//        }

//		public async Task<bool> TrainModelAndUpdateDatabase()
//		{
//			try
//			{
//				List<AllPredictionDataset> allPredLi = new List<AllPredictionDataset>();
//				List<DailyPredictionDataset> dailyPredData = await _predRepo.GetDailyPredictionDataset();
//				foreach (DailyPredictionDataset el in dailyPredData)
//					allPredLi.Add(new AllPredictionDataset(el));
//                bool trained  = await TrainAll(dailyPredData);
//                if (!trained) return false;
//                bool added = await _predRepo.AddToAllPredictionDataset2(allPredLi);
//                if (!added) return false;
//                bool deleted = await _predRepo.DeleteFromDailyPredictionDataset();
//				return (trained && added && deleted);
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex.ToString());
//				return false;
//			}
//		}

//        private async Task<bool> TrainAll(List<DailyPredictionDataset> data)
//        {
//            try
//            {
//                if (data is null) return false;
//                string url = $"{_baseUrl}/train_all_models";
//                HttpClient client = new HttpClient();
//                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
//                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//                var response = await client.PostAsync(url, content);
//                var result = await response.Content.ReadAsStringAsync();
//                JsonElement rootElement = JsonDocument.Parse(result).RootElement;
//                if (rootElement.GetProperty("message").GetString().Equals("Trained successfully", StringComparison.OrdinalIgnoreCase))
//                    return true;
//                return false;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return false;
//            }
//        }




//        //private async Task<string> HitTrainingUrl(string url, List<DailyPrediction> pmLi)
//        //{
//        //    try
//        //    {
//        //        if (pmLi is null)
//        //            pmLi = new List<DailyPrediction>();
//        //        HttpClient client = new HttpClient();
//        //        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(pmLi);
//        //        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//        //        var response = await client.PostAsync(url, content);
//        //        var result = await response.Content.ReadAsStringAsync();
//        //        JsonElement rootElement = JsonDocument.Parse(result).RootElement;
//        //        return "trained successfully";
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError(ex.Message);
//        //        return "0";
//        //    }
//        //}



//     //   public async Task<int> HitPredictUrl(string url, LiveMatchModel match, string keyword = "")
//     //   {
//     //       try
//     //       {
//     //           url = _baseUrl + '/' + url;
//     //           HttpClient client = new HttpClient();
//     //           //List<DailyPrediction> li = new List<DailyPrediction>() { MapLiveMatchToDailyPredictionData(match)};
//     //           //List<LiveMatchModel> li = new List<LiveMatchModel>() { match };
//     //           List<PredictionInputDto> li = new List<PredictionInputDto>()
//     //           {
//     //               MappingService.MapLiveMatchToPredictionInputDto(match)
//     //           };
//     //           var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(li);
//     //           var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//     //           var response = await client.PostAsync(url , content);
//     //           var result = await response.Content.ReadAsStringAsync();

//     //           JsonElement rootElement = JsonDocument.Parse(result).RootElement;
//     //           foreach(var el in rootElement.EnumerateObject())
//     //           {
//     //               if(el.Value.ValueKind == JsonValueKind.Array)
//     //               {
//     //                   foreach (var val in el.Value.EnumerateArray())
//     //                   {
//     //                       if(val.ValueKind == JsonValueKind.Number)
//     //                           return ProcessPred(val.ToString());
//     //                       else
//     //                       {
//     //                           if (val.ToString().Equals("Home"))
//     //                           {
//     //                               if (keyword.Equals("Home"))
//     //                                   return TeamWin();
//     //                               else
//     //                                   return TeamLose();
//     //                           }
//     //                           else if (val.ToString().Equals("Away"))
//     //                           {
//     //                               if (keyword.Equals("Away"))
//     //                                   return TeamWin();
//     //                               else
//     //                                   return TeamLose();
//     //                           }
//     //                           else if (val.ToString().Equals("Draw"))
//     //                           {
//     //                               if (keyword.Equals("Home"))
//     //                                   return DrawHome();
//     //                               else
//     //                                   return DrawAway();
//     //                           }
//     //                           else
//     //                           {
//     //                               _logger.LogWarning($"Not covered case. url is {url} and value is {val} ");
//     //                           }
//     //                       }
//     //                   }
//     //               }

//     //               return ProcessPred(el.Value.ToString());

//     ////               if (el.Value.ValueKind == JsonValueKind.Number)
//     //               //{
//     //               //	return ProcessPred( el.Value.ToString());
//     //               //	int res = (int)float.Parse(el.Value.ToString());
//     //               //	if (res >= 1)
//     //               //	{
//     //               //		return res;
//     //               //	}
//     //               //	return res * 100;
//     //               //}
//     //               //return Unpredictable();
//     //           }
//     //           return Unpredictable();
//     //       }catch(Exception ex)
//     //       {
//     //           _logger.LogError(ex.ToString());
//     //           return Unpredictable();
//     //       }
//     //   }

//        //private List<string> PrepareTrainingUrls()
//        //{
//        //    try
//        //    {
//        //        List<string> trainingUrls = new List<string>();
//        //        string jsonString = File.ReadAllText(_filePath);
//        //        JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
//        //        JsonElement rootElement = jsonDocument.RootElement;
//        //        JsonElement training = rootElement.GetProperty("Prediction").GetProperty("TrainingUrls");
//        //        foreach (JsonProperty element in training.EnumerateObject())
//        //        {
//        //            if (!(element.Name.Equals("Goals") || element.Name.Equals("Corners")))
//        //                trainingUrls.Add(element.Value.ToString());
//        //        }
//        //        List<JsonElement> jsonUrls = new List<JsonElement>()
//        //        {
//        //            training.GetProperty("Goals").GetProperty("Over"),
//        //            training.GetProperty("Goals").GetProperty("Under"),
//        //            training.GetProperty("Corners").GetProperty("Over"),
//        //            training.GetProperty("Corners").GetProperty("Under")
//        //        };
//        //        foreach (JsonElement element in jsonUrls)
//        //        {
//        //            foreach (var prop in element.EnumerateObject())
//        //                trainingUrls.Add(prop.Value.ToString());
//        //        }
//        //        for (int i = 0; i < trainingUrls.Count; i++)
//        //            trainingUrls[i] = _baseUrl + "/" + trainingUrls[i];
//        //        return trainingUrls;
//        //    }
//        //    catch(Exception ex)
//        //    {
//        //        _logger.LogError(ex.ToString());
//        //        return new List<string>();
//        //    }
//        //}


//        //public async Task<bool> UploadDailyPredictionData()
//        //{
//        //    try
//        //    {
//        //        List<LiveMatchModel> matches = await _predRepo.GetAllLiveMatches();
//        //        List<short> dynamicGameStatus = new List<short>() { 2, 4, 5, 8 };
//        //        List<DailyPrediction> dataToSave = new List<DailyPrediction>();
//        //        foreach (var match in matches)
//        //        {
//        //            if (dynamicGameStatus.Contains(match.MatchStatus))
//        //            {
//        //                DailyPrediction dt = new DailyPrediction();
//        //                dt = MapLiveMatchToDailyPredictionData(match);
//        //                dataToSave.Add(dt);
//        //            }
//        //        }
//        //        bool saved = await _predRepo.AddDailyPredictionDataList(dataToSave);
//        //        return saved;
//        //    }catch(Exception ex)
//        //    {
//        //        _logger.LogError(ex.ToString());
//        //        return false;
//        //    }
//        //}



//        //public async Task<bool> DeleteDailyPredictionUploadToAllPredictions()
//        //{
//        //    try
//        //    {
//        //        List<AllPrediction> predLi = new List<AllPrediction>();
//        //        List<DailyPrediction> dailyPredData = await _predRepo.GetDailyPredictionData();

//        //        foreach (DailyPrediction el in dailyPredData)
//        //            predLi.Add(MappingService.MapDailyPredictionToAllPrediction(el));
//        //        bool added = await _predRepo.AddToAllPredictions(predLi);
//        //        bool deleted = await _predRepo.DeleteDailyPrediction();
//        //        return true;

//        //    }catch(Exception ex)
//        //    {
//        //        _logger.LogError(ex.ToString());
//        //        return false;
//        //    }
//        //}






//		//public async Task<Result<List<OpportunitiesResponseDto>>> GetAllOpportunities()
//		//{
//  //          try
//  //          {
//  //              List<LiveMatchModel> matches = await _predRepo.GetAllLiveMatches();
//  //              OpportunitiesRequestDto opReq = new OpportunitiesRequestDto(matches);
//  //              string oppsRes = await HitOpportunities(opReq);
//  //              List<OpportunitiesResponseDto> opps = JsonSerializer.Deserialize<List<OpportunitiesResponseDto>>(oppsRes);
//  //              int count = 0;
//  //              foreach (var opRes in opps) count += opRes.Opportunities.Count;
//  //              return new Result<List<OpportunitiesResponseDto>>(200, true, $"{count}", opps);
//  //          }
//  //          catch(Exception ex)
//  //          {
//  //              _logger.LogError(ex.ToString());
//  //              return new Result<List<OpportunitiesResponseDto>>(500,false, "Internal server error", null); ;
//  //          }
//		//}

//		private async Task<string> HitOpportunities(OpportunitiesRequestDto opReq)
//		{
//			try
//			{
//				HttpClient client = new HttpClient();
//				var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(opReq);
//				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
//                string url = $"{_baseUrl}/get_all_opportunities";
//				var response = await client.PostAsync(url, content);
//				var result = await response.Content.ReadAsStringAsync();
//				JsonElement rootElement = JsonDocument.Parse(result).RootElement;
//                JsonElement opportunities = rootElement.GetProperty("Opportunities");
//				return opportunities.ToString();
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex.Message);
//				return "0";
//			}
//		}

//        public async Task<bool> UpdateInMemoryOpportunities()
//        {
//            try
//            {
//                List<LiveMatchModel> matches = await _predRepo.GetAllLiveMatches();
//                OpportunitiesRequestDto opReq = new OpportunitiesRequestDto(matches);
//                string oppsRes = await HitOpportunities(opReq);
//                List<OpportunitiesResponseDto> opps = JsonSerializer.Deserialize<List<OpportunitiesResponseDto>>(oppsRes);
//                _inMemory.SetAllOpportunitiesDto(opps);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.ToString());
//                return false;
//            }
//        }

//    }
//}