using InPlayWise.Common.DTO;
using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.Enums;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Mappings;
using InPlayWise.Data.DTO;
using InPlayWise.Data.DTO.OpportunitiesEntities;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.Prediction;
using InPlayWise.Data.Entities.PredictionEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace InPlayWise.Core.Services
{
    public class OpportunitiiesPredictionService : IOpportunitiesPredictionService
	{
		private readonly IOpportunitiesPredictionRepository _predRepo;
		private readonly ILogger<OpportunitiiesPredictionService> _logger;
		private readonly IConfiguration _config;
		private readonly IHttpContextService _httpContext;
		private readonly MatchInMemoryService _inMemory;
		private readonly string _baseUrl;

		public OpportunitiiesPredictionService(IOpportunitiesPredictionRepository predRepo, ILogger<OpportunitiiesPredictionService> logger, IConfiguration config, MatchInMemoryService mem, IHttpContextService httpContext)
		{
			_predRepo = predRepo;
			_logger = logger;
			_config = config;
			_inMemory = mem;
			_httpContext = httpContext;
			_baseUrl = _config.GetSection("Prediction:BaseUrl").Value;
		}

		public  Result<List<OpportunitiesResponseDto>> GetAllOpportunities()
		{
			try
			{
				List<OpportunitiesResponseDto> opps = _inMemory.GetAllOpportunitiesDto();
				int count = 0;
				foreach (var opRes in opps) count += opRes.Opportunities.Count;
				return Result<List<OpportunitiesResponseDto>>.Success(msg:count.ToString(), opps);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<List<OpportunitiesResponseDto>>.InternalServerError();
			}
		}

		public async Task<Result<OpportunitiesResponseDto>> GetMatchOpportunities(string matchId)
		{
			try
			{
				//if (!await ValidateHit(matchId))
				//    return new Result<OpportunitiesResponseDto>(400, false, "Limit expired", null);

				List<OpportunitiesResponseDto> allOps = _inMemory.GetAllOpportunitiesDto();
				foreach (OpportunitiesResponseDto op in allOps)
				{
					if (op.MatchId.Equals(matchId))
						return new Result<OpportunitiesResponseDto>(200, true, "opportunities", op);
				}
				return new Result<OpportunitiesResponseDto>(404, true, "Match not found", null);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new Result<OpportunitiesResponseDto>(500, true, "Internal server error", null);
			}
		}



		//public async Task<bool> UploadDailyPredictionDataset()
		//{
		//	try
		//	{
		//		List<LiveMatchModel> matches = (await _predRepo.GetAllLiveMatches());
		//		if (matches is null) return false;
		//		List<DailyPredictionDataset> dataSet = new List<DailyPredictionDataset>();
		//		foreach (LiveMatchModel match in matches)
		//			dataSet.Add(new DailyPredictionDataset(match));
		//		bool uploaded = await _predRepo.AddToDailyPredictionDataset(dataSet);
		//		return uploaded;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.ToString());
		//		return false;
		//	}
		//}

		public async Task<bool> TrainModelAndUpdateDatabase()
		{
			try
			{

				string url = $"{_baseUrl}/train_all_models";
				bool success = false;
				using (var client = new HttpClient())
				{
					client.Timeout = TimeSpan.FromMinutes(30);
					var response = await client.GetAsync(url);
					var result = await response.Content.ReadAsStringAsync();
					JsonElement rootElement = JsonDocument.Parse(result).RootElement;
					if (rootElement.GetProperty("message").GetString().Equals("Trained successfully", StringComparison.OrdinalIgnoreCase))
						success = true;
				}
				return success;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}


        public async Task<bool> UpdateInMemoryOpportunities()
        {
            try
            {
                JsonElement opportunities;
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    string url = $"{_baseUrl}/opportunities";
                    //url = "http://127.0.0.1:5000/test";
                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    JsonElement rootElement = JsonDocument.Parse(result).RootElement;
                    bool oppFound = rootElement.TryGetProperty("Opportunities", out opportunities);
					if (!oppFound) return false;
                }
                string oppsRes = opportunities.ToString();
                List<OpportunitiesResponseDto> opps = JsonSerializer.Deserialize<List<OpportunitiesResponseDto>>(oppsRes);
                List<LiveMatchDto> matches = _inMemory.GetAllLiveMatches();
                foreach (var opp in opps)
                {
                    LiveMatchDto match = matches.SingleOrDefault(m => m.MatchId.Equals(opp.MatchId));
                    if (match is null) continue;
                    opp.MatchId = match.MatchId;
                    opp.HomeTeamName = match.HomeTeamName;
                    opp.AwayTeamName = match.AwayTeamName;
                    opp.HomeGoals = match.HomeGoals;
                    opp.AwayGoals = match.AwayGoals;
                    opp.HomeLogo = match.HomeTeamLogo;
                    opp.AwayLogo = match.AwayTeamLogo;
                    opp.CompetitionName = match.CompetitionName;
                    opp.CompetitionLogo = match.CompetitionLogo;
					opp.CurrentKickoffTime = match.CurrentKickoffTime;
					opp.MatchStatus = match.MatchStatus;
                }
				opps = opps.Where(op => op.HomeTeamName is not null && op.AwayTeamName is not null).ToList();
				_inMemory.SetAllOpportunitiesDto(opps);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        // updation of opportunities complete



        private async Task<bool> ValidateHit(string matchId)
		{
			try
			{
				string userId = _httpContext.GetUserId();
				PlanFeatures planFeatures = await _predRepo.GetPlanFeatures(userId);
				List<PredictionCounter> existingHits = await _predRepo.GetUserHitsOnMatches(userId);
				if (existingHits is null || planFeatures is null) return false;

				int totalCount = 0;
				foreach (PredictionCounter c in existingHits)
				{
					totalCount += c.Hits;
				}

				if (totalCount >= planFeatures.MaxPredictions) return false;

				foreach (PredictionCounter c in existingHits)
				{
					if (c.MatchId.Equals(matchId))
					{
						if (c.Hits >= planFeatures.LivePredictionPerGAme) return false;
						else
						{
							c.Hits++;
							bool updated = await _predRepo.UpdateCounter(c);
							return updated;
						}
					}
				}


				PredictionCounter counter = new PredictionCounter()
				{
					MatchId = matchId,
					UserId = userId,
					Hits = 1
				};
				bool added = await _predRepo.AddCounter(counter);
				return added;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}


		//public async Task<bool> DeleteDailyData()
		//{
		//	try
		//	{
		//		bool deleted = await _predRepo.DeleteFromDailyPredictionDataset();
		//		return deleted;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.ToString());
		//		return false;
		//	}
		//}

		public async Task<Result<Dictionary<string, int>>> GetOpportunitiesCount()
		{
			try
			{
				Dictionary<string, int> result = new Dictionary<string, int>();
				List<OpportunitiesResponseDto> opps = _inMemory.GetAllOpportunitiesDto();
				foreach (var op in opps)
				{
					result.Add(op.MatchId, op.Opportunities.Count);
				}
				return new Result<Dictionary<string, int>>(200, true, "count", result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new Result<Dictionary<string, int>>(500, false, "Internal server error", null);
			}

		}

		public async Task<bool> TrainModelWithAllData()
		{
			try
			{

				return false;

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<bool> SavePredictionRecord()
		{
			try
			{
				List<LiveMatchDto> matches = _inMemory.GetAllLiveMatches();
				List<PredictionRecord> predRec = new List<PredictionRecord>();

				foreach (var match in matches)
				{
					bool condition = (match.MatchMinutes % 5 == 0) && (match.MatchMinutes != 0) && (match.MatchStatus == 2 || match.MatchStatus == 4) && match.MatchMinutes <= 90;
					if (condition)
					{
						var opRes = (await GetMatchOpportunities(match.MatchId)).Items;
						if (opRes is null)
							continue;
						foreach (var opp in opRes.Opportunities)
						{
							PredictionRecord pr = new PredictionRecord()
							{
								Id = Guid.NewGuid(),
								MatchId = opRes.MatchId,
								OpportunityName = opp.Name,
								PredictionProb = opp.Prediction,
								MatchMinute = match.MatchMinutes
							};
							predRec.Add(pr);
						}
					}
				}
				bool saved = await _predRepo.SavePredictionRecords(predRec);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}


		public async Task<List<PredictionRecord>> GetPredictionRecordOfMatch(string matchId)
		{
			try
			{
				return await _predRepo.GetRecordByMatchId(matchId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}


		public async Task<List<string>> GetMatchesIdWithRecord()
		{
			try
			{
				return await _predRepo.GetMatchesIdWithRecords();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<OpportunitiesRequestDto> GetFormattedDataForPrediction()
		{
			try
			{
				List<LiveMatchModel> matches = (await _predRepo.GetAllLiveMatches()).Take(50).ToList();
				OpportunitiesRequestDto opReq = new OpportunitiesRequestDto(matches);
				return opReq;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		//public async Task<bool> UploadToActiveMatchPrediction()
		//{
		//	try
		//	{
		//		List<LiveMatchDto> matches = _inMemory.GetAllLiveMatches();
		//		List<PredictionActiveMatchesData> AllPred = new List<PredictionActiveMatchesData>();
		//		foreach (LiveMatchDto match in matches)
		//		{
		//			bool condition = (!(match.MatchStatus == 2 || match.MatchStatus == 4)) || match.MatchMinutes < 3 || match.HomeAttacks == 0 || match.AwayAttacks == 0;
		//			if (condition)
		//				continue;
		//			PredictionActiveMatchesData predictionData = MappingService.MapLiveMatchDataToPredictionActiveMatchData(match);
		//			AllPred.Add(predictionData);
		//		}
		//		bool saved = await _predRepo.UploadToActiveMatchesPrediction(AllPred);
		//		return saved;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.ToString());
		//		return false;
		//	}
		//}


		//public async Task<bool> AddToFullPrediction(string matchId)
		//{
		//	try
		//	{
		//		LiveMatchDto match = _inMemory.GetAllLiveMatches().Where(m => m.MatchId.Equals(matchId)).SingleOrDefault();
		//		if (match is null) return false;
		//		List<PredictionActiveMatchesData> predData = await _predRepo.GetPredictionDatasetForActiveMatch(matchId);
		//		List<PredictionFullData> predFullData = new List<PredictionFullData>();
		//		foreach (PredictionActiveMatchesData pred in predData)
		//		{
		//			PredictionFullData pdf = MappingService.MapActiveMatchDataToPredictionFullData(pred);

		//			pdf.HomeTeamHalfTimeScore = match.HomeTeamHalfTimeScore;
		//			pdf.AwayTeamHalfTimeScore = match.AwayTeamHalfTimeScore;

		//			pdf.HomeTeamFullTimeScore = match.HomeGoals;
		//			pdf.AwayTeamFullTimeScore = match.AwayGoals;

		//			pdf.AwayTeamFullTimeCorners = match.AwayCorners;
		//			pdf.HomeTeamFullTimeCorners = match.HomeCorners;

		//			predFullData.Add(pdf);

		//		}
		//		bool saved = await _predRepo.UploadToFullPredictionData(predFullData);
		//		return saved;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.ToString());
		//		return false;
		//	}
		//}

        public async Task<bool> CalculatePred(LiveMatchModel liveMatch)
        {
            try
            {
                int totalGoals = liveMatch.AwayGoals + liveMatch.HomeGoals;
                string matchId = liveMatch.MatchId;

                // Await the task to get the list of prediction records
                List<PredictionRecord> predRecords = await GetPredictionRecordOfMatch(matchId);

                // Group the prediction records by OpportunityName
                var groupedRecords = predRecords.GroupBy(record => record.OpportunityName);

                // List to store PredictionAccuracy objects
                List<PredictionAccuracy> predictionAccuracies = new List<PredictionAccuracy>();

                // Calculate the average PredictionProb for each group and determine accuracy
                foreach (var group in groupedRecords)
                {
                    var sum = group.Sum(record => record.PredictionProb);
                    var count = group.Count();
                    var averagePredictionProb = sum / count;

                    // Extract the threshold from the OpportunityName (assuming format like "Under 2.5 goals")
                    string[] parts = group.Key.Split(' ');
                    if (parts.Length == 3 && float.TryParse(parts[1], out float threshold))
                    {
                        string condition = parts[0].ToLower(); // Convert to lower case for case insensitive comparison

                        int correctPredictions = 0;
                        int totalPredictions = group.Count();

                        foreach (var record in group)
                        {
                            bool prediction = record.PredictionProb >= 50;
                            bool actual = false;

                            if (condition.Equals("under"))
                            {
                                actual = totalGoals < threshold;
                            }
                            else if (condition.Equals("over"))
                            {
                                actual = totalGoals >= threshold;
                            }

                            if (prediction == actual)
                            {
                                correctPredictions++;
                            }
                        }

                        float accuracy = ((float)correctPredictions / totalPredictions) * 100;

                        // Create a new PredictionAccuracy object and add it to the list
                        var predictionAccuracy = new PredictionAccuracy
                        {
                            Id = Guid.NewGuid(),
                            MatchId = matchId,
                            EndTime = DateTime.Now, // Assuming EndTime is the current time
                            Opportunity = group.Key,
                            Prediction = averagePredictionProb,
                            Accuracy = accuracy
                        };

                        predictionAccuracies.Add(predictionAccuracy);
                    }
                }

                // Save all PredictionAccuracy objects after calculations are complete
                bool savedPredictionAccuracies = await _predRepo.SavePredictionAccuracies(predictionAccuracies);
                if (!savedPredictionAccuracies)
                {
                    _logger.LogError($"Failed to save PredictionAccuracies for MatchId: {matchId}");
                    return false;
                }

                // Delete the prediction records after successfully saving the accuracies
                await _predRepo.DeletePredictionRecord(matchId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating predictions.");
                return false;
            }
        }


        public async Task<Result<AccuracyDto>> GetAccuracyByMatchIdAsync(string matchId, OpportunityType type, float value, OpportunityEvent oppEvent)
        {
			try
			{
                string opportunity = $"{type} {value} {oppEvent}";
                float accuracy = await _predRepo.GetAccuracyByMatchIdAsync(matchId,opportunity);
				
                AccuracyDto accuracyDto = new AccuracyDto
                {
                    AccuracyPercentage = accuracy
                };
                if (accuracy == 0.0f)
                {
					return Result<AccuracyDto>.NotFound("No such Match and Opportunity found");

                }

				return Result<AccuracyDto>.Success("Accuracy is fetched successfully", accuracyDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<AccuracyDto>.InternalServerError("An error occurred while retrieving the accuracy.");
            }
        }

        public async Task<Result<AccuracyDto>> GetAccuracyByDate(DateTime date, OpportunityType type, float value, OpportunityEvent oppEvent)
        {
            try
            {
                if (date > DateTime.UtcNow)
                {
                    return Result<AccuracyDto>.BadRequest("The date cannot be in the future.");
                }
                string opportunity = $"{type} {value} {oppEvent}";
                var accuracy = await _predRepo.GetAccuracyByDate(date, opportunity);
                var accuracyDto = new AccuracyDto
                {
                    AccuracyPercentage = accuracy
                };

                if (accuracy == 0.0f)
                {
                    return Result<AccuracyDto>.NotFound("No Opportunity found");

                }

				return Result<AccuracyDto>.Success("Accuracy is fetched successfully",accuracyDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
				return Result<AccuracyDto>.InternalServerError("An error occurred while retrieving the accuracy.");
            }
        }

        public async Task<Result<List<MatchAccuracyDto>>> GetMatchListAccuracy(DateTime date)
        {

            try
            {
                if (date > DateTime.UtcNow)
                {
                    return Result<List<MatchAccuracyDto>>.BadRequest("The date cannot be in the future.");
                }
                
                var pastGamesAccuracies = await _predRepo.GetMatchListAccuracy(date);

                var matchAccuracyDtos = pastGamesAccuracies.Select(pa => new MatchAccuracyDto
                {
                    MatchId = pa.MatchId,
                    Opportunity = pa.Opportunity,
                    Accuracy = pa.Accuracy
                }).ToList();

                 return Result<List<MatchAccuracyDto>>.Success("Accuracy is fetched successfully", matchAccuracyDtos);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<MatchAccuracyDto>>.InternalServerError("An error occurred while retrieving the accuracy.");
            }
        }

        public async  Task<Result<List<AccuracyPerBetDto>>> GetAccuracyPerBet()
        {
			try
			{
                var allAccuracies = await _predRepo.GetAllAccuracies();
                if (!allAccuracies.Any())
                {
                    return Result<List<AccuracyPerBetDto>>.NotFound("No Opportunity found");

                }
                var accuracyPerBet = allAccuracies
            .GroupBy(pa => pa.Opportunity)
            .Select(g => new AccuracyPerBetDto
            {
                Opportunity = g.Key,
                Accuracy = g.Average(pa => pa.Accuracy)
            })
            .ToList();

                return Result<List<AccuracyPerBetDto>>.Success("Accuracy is fetched successfully",accuracyPerBet);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<AccuracyPerBetDto>>.InternalServerError("An error occurred while retrieving the accuracy.");
            }

        }

        public async Task<Result<AccuracyDto>> GetTotalAccuracy()
        {
			try
			{
                var allAccuracies = await _predRepo.GetAllAccuracies();
				if (!allAccuracies.Any())
				{
                    return Result<AccuracyDto>.NotFound("No Opportunity found");
                }
                var averageAccuracy = allAccuracies.Any()
            ? allAccuracies.Average(pa => pa.Accuracy)
            : 0.000f; 

                // Map to AccuracyDto
                var accuracyDto = new AccuracyDto
                {
                    AccuracyPercentage = averageAccuracy
                };

                return Result<AccuracyDto>.Success("Accuracy is fetched successfully",accuracyDto);

            }
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
                return Result<AccuracyDto>.InternalServerError("An error occurred while retrieving the accuracy.");
            }
            
        }

        public async Task<bool> RemoveEndedOpportunities()
        {
            try
            {
				List<LiveMatchDto> matches = _inMemory.GetAllLiveMatches();
				List<string> existingMatchIds = matches.Select(m => m.MatchId).ToList();
				List<OpportunitiesResponseDto> allOps = _inMemory.GetAllOpportunitiesDto();
				List<OpportunitiesResponseDto> res = allOps.Where(op => existingMatchIds.Contains(op.MatchId)).ToList();
				foreach(OpportunitiesResponseDto matchOpps in res)
				{
					LiveMatchDto match = matches.First(match => match.MatchId.Equals(matchOpps.MatchId));
					int totalGoals = match.HomeGoals + match.AwayGoals;
                    List<Opportunity> validOpps = new List<Opportunity>();
                    foreach (Opportunity opp in matchOpps.Opportunities)
					{
						if (opp.Name.Contains("goals"))
						{
                            int value = (int)Math.Ceiling(opp.Name.Split(' ')
								.Where(part => double.TryParse(part, out _))
								.Select(double.Parse).FirstOrDefault());

							if(value != totalGoals)
							{
								validOpps.Add(opp);
							}
                        }
					}
					matchOpps.Opportunities = validOpps;
				}

				_inMemory.SetAllOpportunitiesDto(res);
				return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
				return false;
                //return Result<AccuracyDto>.InternalServerError("An error occurred while retrieving the accuracy.");
            }
        }
    }
}
