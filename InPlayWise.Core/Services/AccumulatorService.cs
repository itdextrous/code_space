//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Common.Wrappers;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Mappings;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class AccumulatorService : IAccumulatorService
    {
        private readonly MatchInMemoryService _matchinMemoryService;
        private readonly IAccumulaterRepository _accumulatorRepository;
        private readonly ILogger<AccumulatorService> _logger;
        private readonly IHttpContextService _httpContext;
        public AccumulatorService(IAccumulaterRepository accumulatorRepository, ILogger<AccumulatorService> logger, IHttpContextService httpContext, MatchInMemoryService matchInMemoryService)
        {
            _accumulatorRepository = accumulatorRepository;
            _logger = logger;
            _httpContext = httpContext;
            _matchinMemoryService = matchInMemoryService;
        }

        public async Task<Result<List<List<Accumulator>>>> GetSavedAccumulatorsAsync()
        {
            try
            {
                // Get the user ID
                string? userId = _httpContext.GetUserId();
                 string?  productId = _httpContext.GetProductId();
                int  limit = _accumulatorRepository.GetAccumulatorHistoryLimit(productId).Result;
                limit = 1000; // no limit without feature counter
                
                // Check if user is authenticated
                if (userId == null)
                {
                    return Result<List<List<Accumulator>>>.Unauthorized("User is not authenticated.");
                }

                // Retrieve saved accumulators from the repository based on user ID
                var groupedAccumulators = await _accumulatorRepository.GetSavedAccumulatorsAsync(userId);

                //check if groupedAccumulators are null
                if(groupedAccumulators== null)
                {
                    return Result<List<List<Accumulator>>>.InternalServerError();
                }

                List<LiveMatchDto> liveMatches = _matchinMemoryService.GetAllLiveMatches();

                foreach (List<Accumulator> group in groupedAccumulators) {
                    foreach (Accumulator acc in group)
                    {
                        LiveMatchDto match = liveMatches.FirstOrDefault(match => match.MatchId.Equals(acc.MatchId));
                        if (match != null)
                        {
                            acc.MatchStatus = match.MatchStatus;
                            acc.KickoffTimeStamp = match.CurrentKickoffTime;
                        }

                        if (acc.MatchStatus == 0 && acc.OppResult == 0)
                        {
                            int totalGoals = acc.HomeTeamGoals + acc.AwayTeamGoals;
                            bool isFinal = true;
                            string oppName = acc.OpportunityName;
                            string[] parts = oppName.Split(' ');

                            if (parts.Length > 1 && float.TryParse(parts[1], out float goalValue))
                            {
                                bool result = false;
                                if (oppName.StartsWith("over", StringComparison.OrdinalIgnoreCase))
                                {

                                    if (totalGoals > goalValue)
                                    {
                                        result = true;
                                        acc.OppResult = OpportunityResult.Won;
                                    }
                                    else if (isFinal)
                                    {
                                        acc.OppResult = OpportunityResult.Lost;
                                    }
                                }
                                else if (oppName.StartsWith("under", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (totalGoals > goalValue)
                                    {
                                        result = true;
                                        acc.OppResult = OpportunityResult.Lost;
                                    }
                                    else if (isFinal)
                                    {
                                        acc.OppResult = OpportunityResult.Won;
                                    }
                                }
                            }
                        }
                    }
                }

                return Result<List<List<Accumulator>>>.Success("Accumulators retrieved successfully.", groupedAccumulators);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving saved accumulators.");
                return Result<List<List<Accumulator>>>.InternalServerError("An error occurred while retrieving accumulators.");
            }
        }




        public async Task<Result<bool>> SaveAccumulatorsAsync(List<OpportunityDto> opportunityDtos)
        {
            // Check if the OpportunityDto is null
            if (opportunityDtos == null)
            {
                return Result<bool>.BadRequest("Opportunities Can't be null");
            }

            try
            {
                // Get the user ID from the HttpContext
                var userId = _httpContext.GetUserId();
                //bool allAccumulatorsExist = await _accumulatorRepository.CheckIfAccumulatorsExistAsync(opportunityDtos,userId);
                bool allAccumulatorsExist = (await IfAccumulatoExists(opportunityDtos, userId)) != Ret.False;
                if (allAccumulatorsExist)
                {
                    return Result<bool>.BadRequest("This accumulator is already saved");
                }

                var liveGames = _matchinMemoryService.GetAllLiveMatches();

                // Convert the live games list to a dictionary for faster lookups (optional optimization)
                var liveGamesDictionary = liveGames.ToDictionary(game => game.MatchId);

                //var productId = GetProductId();
                //var limit = _accumulatorRepository.GetGeneratedAccumulatorLimit(productId).Result;

                // If user ID is null, return unauthorized result
                if (userId == null)
                {
                    return Result<bool>.Unauthorized("User is not authenticated.");
                }

                var duplicateMatchIds = opportunityDtos
                     .GroupBy(dto => dto.MatchId)
                     .Where(group => group.Count() > 1)
                     .Select(group => group.Key)
                     .ToList();

                if (duplicateMatchIds.Any())
                {
                    // If there are duplicate MatchIds, return bad request with the first duplicate found
                    var firstDuplicateMatchId = duplicateMatchIds.First();
                    return Result<bool>.BadRequest($"Duplicate MatchId '{firstDuplicateMatchId}' found in the list.");
                }

                var accumulators = new List<Accumulator>();

                foreach (var opportunityDto in opportunityDtos)
                {

                    // Map OpportunityDto to Accumulator
                    var accumulator = MappingService.MapToAccumulator(opportunityDto);
                    accumulator.UserId = userId;
                    if (liveGamesDictionary.TryGetValue(opportunityDto.MatchId, out var liveGame))
                    {
                        // If a match is found, map the HomeTeam and AwayTeam names
                        accumulator.HomeTeamName = liveGame.HomeTeamName;
                        accumulator.AwayTeamName = liveGame.AwayTeamName;
                        accumulator.HomeTeamGoals = liveGame.HomeGoals;
                        accumulator.AwayTeamGoals = liveGame.AwayGoals;
                    }
                    accumulators.Add(accumulator);
                }

                // Save accumulators to the repository
                await _accumulatorRepository.SaveAccumulatorsAsync(accumulators);

                // Return success result
                return Result<bool>.Success("Accumulators saved successfully.", true);
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError(ex, "Error saving accumulators");

                // Return internal server error result
                return Result<bool>.InternalServerError("An error occurred while saving accumulators.");
            }
        }


        //p0
        private async Task<Ret> IfAccumulatoExists(List<OpportunityDto> opportunityDtos, string userId)
        {
            try
            {
                List<string> matchIdLi = opportunityDtos.Select(dto => dto.MatchId.Trim().ToLower()).ToList();
                List<string> opportunityNamesLi = opportunityDtos.Select(dto => dto.OpportunityName.Trim().ToLower()).ToList();
                List<List<Accumulator>> savedGroups = await _accumulatorRepository.GetAccumulatorGroupsByUserId(userId);
                HashSet<string> matchIdSet = new HashSet<string>(matchIdLi);
                HashSet<string> opportunityNamesSet = new HashSet<string>(opportunityNamesLi);
                foreach (var accumulator in savedGroups)
                {
                    if(accumulator.Count == opportunityDtos.Count)
                    {
                        if(matchIdSet.SetEquals(new HashSet<string>(accumulator.Select(ac => ac.MatchId).ToList())))
                        {
                            HashSet<string> current_opp_names = new HashSet<string>(accumulator.Select(ac => ac.OpportunityName.ToLower().Trim()).ToList());
                            if (opportunityNamesSet.SetEquals(current_opp_names)){
                                return Ret.True;
                            }
                        }
                    }
                }
                return Ret.False;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Ret.Error;
            }
        }

        
        public Result<List<List<HedgeResponseDto>>> CreateHedges(List<HedgeDto> hedges)
        {
            try
            {
                // Check if the list of hedges is null or empty
                if (hedges == null || hedges.Count == 0)
                {
                    return Result<List<List<HedgeResponseDto>>>.BadRequest("There are no opportunities to create hedges.");
                }

                // Calculate the number of combinations based on the number of hedges and the 'WillLose' property of the first hedge
                int combinationsCount = hedges.Count - hedges.First().WillLose;

                // Map hedge DTOs to hedge response DTOs
                var hedgeResponses = hedges.Select(h => MappingService.MaptoHedgeResponse(h)).ToList();

                // Check if mapping resulted in any null hedge response DTOs
                if (hedgeResponses.Any(hr => hr == null))
                {
                    return Result<List<List<HedgeResponseDto>>>.InternalServerError("Error mapping hedgeDto objects to hedgeResponseDto objects.");
                }

                // Check if the number of combinations is valid
                if (combinationsCount <= 0)
                {
                    return Result<List<List<HedgeResponseDto>>>.BadRequest("Will lose should be less than the number of opportunities selected.");
                }

                // Generate combinations
                var combinations = new List<List<HedgeResponseDto>>();
                GenerateCombinations(hedgeResponses, new List<HedgeResponseDto>(), combinations, combinationsCount, 0);

                // Return success result with created hedge combinations
                return Result<List<List<HedgeResponseDto>>>.Success("Hedges are created successfully.", combinations);
            }
            catch (Exception ex)
            {
                // Log and return internal server error if an exception occurs
                _logger.LogError(ex, "An error occurred while creating hedges.");
                return Result<List<List<HedgeResponseDto>>>.InternalServerError();
            }
        }


        /// <summary>
        /// Recursively generates combinations of hedge response DTOs.
        /// </summary>
        /// <param name="hedgeResponses">The list of hedge response DTOs.</param>
        /// <param name="currentCombination">The current combination being generated.</param>
        /// <param name="combinations">The list to store generated combinations.</param>
        /// <param name="combinationsCount">The desired number of combinations.</param>
        /// <param name="currentIndex">The current index in the list of hedge response DTOs.</param>
        private void GenerateCombinations(List<HedgeResponseDto> hedgeResponses, List<HedgeResponseDto> currentCombination, List<List<HedgeResponseDto>> combinations, int combinationsCount, int currentIndex)
        {
            // Check if the current combination is complete
            if (currentCombination.Count == combinationsCount)
            {
                currentCombination = currentCombination.OrderByDescending(item => item.ConfidencyLevel).ToList();
                // Add the current combination to the list of combinations
                combinations.Add(currentCombination.ToList());
                return;
            }

            // Iterate through remaining hedge response DTOs to generate combinations
            for (int i = currentIndex; i < hedgeResponses.Count; i++)
            {
                // Add the current hedge response DTO to the combination
                currentCombination.Add(hedgeResponses[i]);

                // Generate combinations recursively starting from the next index
                GenerateCombinations(hedgeResponses, currentCombination, combinations, combinationsCount, i + 1);

                // Remove the last hedge response DTO to backtrack
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        public async Task<Result<bool>> SaveHedgesAsync(List<List<HedgeDto>> hedges)
        {
            try
            {
                // Initialize a list to hold lists of opportunity DTOs
                var opportunitiesLists = new List<List<OpportunityDto>>();

                // Iterate through each hedge collection
                foreach (var hedgeList in hedges)
                {
                    // Initialize a list to hold opportunity DTOs
                    var opportunities = new List<OpportunityDto>();

                    // Map each hedge DTO to an opportunity DTO and add it to the list
                    foreach (var hedge in hedgeList)
                    {
                        var opportunityDto = MappingService.MapToOpportunityDto(hedge);
                        opportunityDto.IsHedged = true;
                        opportunities.Add(opportunityDto);
                    }

                    // Save the list of opportunity DTOs asynchronously
                    var saveResult = await SaveAccumulatorsAsync(opportunities);

                    if (!saveResult.IsSuccess)
                    {
                        return Result<bool>.BadRequest("Hedge is already saved");
                    }
                }

                return Result<bool>.Success("Hedges saved successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving hedges.");
                return Result<bool>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteAccumulaterAsync(string groupId)
        {
            try
            {
                // Delete the accumulator asynchronously from the repository
                bool isSuccess = await _accumulatorRepository.DeleteAccumulatorAsync(groupId);

                // Check if the deletion was successful
                if (isSuccess)
                {
                    return Result<bool>.Success("Accumulators deleted successfully.", true);
                }
                else
                {
                    return Result<bool>.BadRequest("No accumulators exist for the given group ID.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting accumulators.");
                return Result<bool>.InternalServerError();
            }
        }

     
        public async Task<bool> UpdateAccumulatorResult(LiveMatchModel liveMatch)
        {
            try
            {
                List<Accumulator> matchOpps = await _accumulatorRepository.GetAllOpps(liveMatch.MatchId);
                if (!matchOpps.Any())
                {
                    return false;
                }

                int totalGoals = liveMatch.AwayGoals + liveMatch.HomeGoals;

                // Array of bad statuses
                int[] badStatus = new int[] { 0, 8, 9, 10, 12, 13 };

                // Determine if the status is final based on bad statuses
                bool isFinal = badStatus.Contains(liveMatch.MatchStatus);

                foreach (var opp in matchOpps)
                {
                    string oppName = opp.OpportunityName;
                    string[] parts = oppName.Split(' ');
                    opp.HomeTeamGoals = liveMatch.HomeGoals;
                    opp.AwayTeamGoals = liveMatch.AwayGoals;
                    opp.MatchEndMinutes = liveMatch.MatchMinutes;
                    if(new List<int>() { 5, 6, 7 }.Contains(liveMatch.MatchStatus)) {
                        opp.MatchEndMinutes = 120;
                    }
                    if (opp.MatchEndMinutes == 0) { opp.MatchEndMinutes = 90; }
                    if (parts.Length > 1 && float.TryParse(parts[1], out float goalValue))
                    {
                        bool result = false;
                        if (oppName.StartsWith("over", StringComparison.OrdinalIgnoreCase))
                        {
                           
                            if (totalGoals > goalValue)
                            {
                                result = true;
                                opp.OppResult = OpportunityResult.Won;
                            }
                            else if (isFinal)
                            {
                                opp.OppResult = OpportunityResult.Lost;
                            }
                            if (isFinal || result )
                            {
                                opp.ResultTime = liveMatch.MatchMinutes;
                            }
                        }
                        else if (oppName.StartsWith("under", StringComparison.OrdinalIgnoreCase))
                        {
                            if (totalGoals > goalValue)
                            {
                                result = true;
                                opp.OppResult = OpportunityResult.Lost;
                            }
                            else if (isFinal)
                            {
                                opp.OppResult = OpportunityResult.Won;
                            }
                            if (isFinal || result)
                            {
                                opp.ResultTime = liveMatch.MatchMinutes;
                            }
                        }
                    }
                }

                // Update the accumulators in the repository
                await _accumulatorRepository.UpdateAccumulator(matchOpps);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating accumulator result");
                return false;
            }
        }



    }
}

