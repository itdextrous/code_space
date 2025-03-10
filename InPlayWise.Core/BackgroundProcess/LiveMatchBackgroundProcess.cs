using System.Reflection;
using AutoMapper;
using InPlayWise.Common.DTO;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Mappings;
using InPlayWise.Core.Services;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using Microsoft.Extensions.Logging;
using Stripe;

namespace InPlayWise.Core.BackgroundProcess
{
    public class LiveMatchBackgroundProcess : ILiveMatchBackgroundProcess
    {
        private readonly IAccumulatorService _accumulator;
        private readonly IBasicDataServices _basicDataService;
        private readonly IBasicInfoServices _basicInfo;
        private readonly ILiveMatchRepository _liveRepo;
        private readonly ILogger<LiveMatchService> _logger;
        private readonly MatchInMemoryService _inMemoryMatch;
        private readonly IOpportunitiesPredictionService _predService;
        private readonly IInsightService _insights;
        private readonly IHistoricMatchBackgroundProcess _historicalMatch;
        private readonly IMapper _mapper;
        private readonly ICleverLabelServices _cleverLabelServices;


        public LiveMatchBackgroundProcess(IBasicDataServices basicDataServices, ILiveMatchRepository liveRepo, ILogger<LiveMatchService> logger, MatchInMemoryService inMemoryMatch, IBasicInfoServices basicInfo, IOpportunitiesPredictionService predService, IInsightService insights, IHistoricMatchBackgroundProcess historicalMatch, IMapper mapper,IAccumulatorService accumulator, ICleverLabelServices cleverLabelServices)
        {
            _basicDataService = basicDataServices;
            _liveRepo = liveRepo;
            _logger = logger;
            _inMemoryMatch = inMemoryMatch;
            _basicInfo = basicInfo;
            _predService = predService;
            _insights = insights;
            _historicalMatch = historicalMatch;
            _mapper = mapper;
            _accumulator = accumulator;
            _cleverLabelServices = cleverLabelServices;
        }

        public async Task<bool> UploadAndUpdateLiveMatches()
        {
            try
            {
                List<LiveMatchModel> liveMatches = await _basicDataService.RealTimeData();
                List<LiveMatchModel> existingMatches = await _liveRepo.GetAllLiveMatches();
                _inMemoryMatch.SetPreviousLiveMatches(existingMatches.Select(match => _mapper.Map<LiveMatchDto>(match)).ToList());
                List<string> matchIdToDelete = new List<string>();
                if (existingMatches is null || liveMatches is null) return false;
                int[] badStatus = new int[] { 0, 1, 8, 9, 10, 13, 12 };
                //int time = 0;
                long start_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (liveMatches.Count == 0)
                {
                    if(existingMatches.Count > 7)
                    {
                        return false;
                    }
                }
                foreach (var liveMatch in liveMatches)
                {

                    LiveMatchModel existingMatch = existingMatches.FirstOrDefault(match => match.MatchId.Equals(liveMatch.MatchId));
                    if (existingMatch is null && badStatus.Contains(liveMatch.MatchStatus)) continue;


                    if(existingMatch is null)
                    {
                        if (badStatus.Contains(liveMatch.MatchStatus))
                            continue;

                        await CompleteMatchInfo(liveMatch);
                        if (string.IsNullOrEmpty(liveMatch.HomeTeamId) || string.IsNullOrEmpty(liveMatch.AwayTeamId))
                        {
                            continue;
                        }
                        bool addToRecent = await _liveRepo.AddToRecentMatch(MappingService.MapLiveToRecent(liveMatch));
                        bool addToLive = await _liveRepo.AddLiveMatch(liveMatch);
                        await RefreshFilters();
                    }
                    else
                    {
                        
                        await MapExistingMatchInfoToLive(liveMatch, existingMatch);
                        UpdateShotsAndCornersTime(liveMatch, existingMatch);
                        MapOpportunities(liveMatch);
                        if ((liveMatch.HomeGoals + liveMatch.AwayGoals) > (existingMatch.HomeGoals + existingMatch.AwayGoals))
                        {
                            await _accumulator.UpdateAccumulatorResult(liveMatch);
                        }
                        if (badStatus.Contains(liveMatch.MatchStatus))
                        {
                            matchIdToDelete.Add(liveMatch.MatchId);
                            //await _predService.CalculatePred(liveMatch); // this update took time
                            await _accumulator.UpdateAccumulatorResult(existingMatch);
                        }
                    }
                }
                bool updated = await _liveRepo.UpdateLiveMatchesRange(liveMatches);
                bool updateInMemory = await UpdateInMemoryMatches();
				existingMatches.ForEach( match =>
                {
                    if (!liveMatches.Select(lm => lm.MatchId).ToList().Contains(match.MatchId) && !matchIdToDelete.Contains(match.MatchId))
                    { matchIdToDelete.Add(match.MatchId); }
                });
                bool deleted = await DeleteMatches(matchIdToDelete);
            
                return updated && deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        private async Task<bool> DeleteMatches(List<string> matchIds)
        {
            try
            {
                foreach(string matchId in matchIds) {
					LiveMatchModel match = await _liveRepo.GetLiveMatchById(matchId);
					if (match is null) return false;
					//bool predUpdated = await _predService.AddToFullPrediction(match.MatchId);
					RecentMatchModel endedMatch = MappingService.MapLiveToRecent(match);
					endedMatch.Ended = true;
                    await _historicalMatch.CompleteMatchInfo(endedMatch);
					bool updateRecent = await _liveRepo.UpdateRecentMatch(endedMatch);
                    //_inMemoryMatch.GetRecentMatches().Add(endedMatch);
                    _inMemoryMatch.AddPastMatchHash(endedMatch);
					await _liveRepo.DeleteLiveMatchById(match.MatchId);
                    bool updateInMemory = await UpdateInMemoryMatches();
                }
                if (matchIds.Count > 0) {
                    await RefreshFilters();
                }

                return true;
			}
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }


		private void MapOpportunities(LiveMatchModel match)
        {
            try
            {
                foreach (var op in _inMemoryMatch.GetAllOpportunitiesDto())
                {
                    if (op.MatchId.Equals(match.MatchId))
                    {
                        match.NumOfOpportunities = op.Opportunities.Count;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private async Task<bool> UpdateInMemoryMatches()
        {
            try
            {
                List<LiveMatchModel> dbMatches = await _liveRepo.GetAllLiveMatches();
                List<LiveMatchDto> matches = new List<LiveMatchDto>();
                foreach (var dbMatch in dbMatches)
                {
                    LiveMatchDto match = _mapper.Map<LiveMatchDto>(dbMatch) ;
                    if (match != null) { matches.Add(match); }
                }
                await AttachInsightsToMatches(matches);
                _inMemoryMatch.SetMatches(matches);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        private bool UpdateShotsAndCornersTime(LiveMatchModel liveMatch, LiveMatchModel existingMatch)
        {
            liveMatch.HomeCornerMinutes = existingMatch.HomeCornerMinutes;
            liveMatch.AwayCornerMinutes = existingMatch.AwayCornerMinutes;
            liveMatch.HomeShotsMinutes = existingMatch.HomeShotsMinutes;
            liveMatch.AwayShotsMinutes = existingMatch.AwayShotsMinutes;
            liveMatch.HomeShotsOnTargetMinutes = existingMatch.HomeShotsOnTargetMinutes;
            liveMatch.AwayShotsOnTargetMinutes = existingMatch.AwayShotsOnTargetMinutes;

            string tm = liveMatch.MatchMinutes.ToString();

            List<int> goodStatus = new List<int>() { 2, 4 };

            if (!goodStatus.Contains(liveMatch.MatchStatus))
                return true;

            if (liveMatch.HomeCorners > existingMatch.HomeCorners)
                liveMatch.HomeCornerMinutes += existingMatch.HomeCornerMinutes.Equals("") ? tm : "," + tm;

            if (liveMatch.AwayCorners > existingMatch.AwayCorners)
                liveMatch.AwayCornerMinutes += existingMatch.AwayCornerMinutes.Equals("") ? tm : "," + tm;

            if (liveMatch.HomeShotsOffTarget + liveMatch.HomeShotsOnTarget > existingMatch.HomeShotsOffTarget + existingMatch.HomeShotsOnTarget)
                liveMatch.HomeShotsMinutes += existingMatch.HomeShotsMinutes.Equals("") ? tm : "," + tm;

            if (liveMatch.AwayShotsOffTarget + liveMatch.AwayShotsOnTarget > existingMatch.AwayShotsOffTarget + existingMatch.AwayShotsOnTarget)
                liveMatch.AwayShotsMinutes += existingMatch.AwayShotsMinutes.Equals("") ? tm : "," + tm;

            if (liveMatch.HomeShotsOnTarget > existingMatch.HomeShotsOnTarget)
                liveMatch.HomeShotsOnTargetMinutes += existingMatch.HomeShotsOnTargetMinutes.Equals("") ? tm : "," + tm;

            if (liveMatch.AwayShotsOnTarget > existingMatch.AwayShotsOnTarget)
                liveMatch.AwayShotsOnTargetMinutes += existingMatch.AwayShotsOnTargetMinutes.Equals("") ? tm : "," + tm;

            return true;
        }



        private async Task<LiveMatchModel> CompleteMatchInfo(LiveMatchModel match)
        {
            try
            {
                List<ApiRecentMatch> recentMatchResponse = await _basicDataService.MatchRecent(match.MatchId);

                if (recentMatchResponse.Count == 0)
                {
                    return null;
                }
                ApiRecentMatch matchRecent = recentMatchResponse[0];
                Competition competition = await _liveRepo.GetCompetitionById(matchRecent.CompetitionId);
                match.CompetitionId = matchRecent?.CompetitionId;
                match.SeasonId = matchRecent.SeasonId;
                //if (competition.CompetitionCategories is not null)
                //{
                //    competition.CompetitionCategories.ForEach(cc => match.CompetititionCategory += cc.Name + ",");
                //}
                //match.CompetititionCategory = match.CompetititionCategory.TrimEnd(',');

                Team homeTeam = await _liveRepo.GetTeamById(matchRecent?.HomeTeamId);
                match.HomeTeamId = matchRecent.HomeTeamId;

                Team awayTeam = await _liveRepo.GetTeamById(matchRecent?.AwayTeamId);
                match.AwayTeamId = matchRecent.AwayTeamId;

                match.HomeTeamRank = string.IsNullOrEmpty(matchRecent.HomePosition) ? 0 : int.Parse(matchRecent.HomePosition);
                match.AwayTeamRank = string.IsNullOrEmpty(matchRecent.AwayPosition) ? 0 : int.Parse(matchRecent.AwayPosition);

                string stageId = matchRecent.Round.StageId;
                string stageName = "";

                match.StageName = stageName;
                match.RoundNumber = matchRecent.Round.RoundNum;
                match.GroupNumber = matchRecent.Round.GroupNum;
                match.CompetitionType = competition.Type;

                

                match.MatchStartTimeOfficial = DateTimeOffset.FromUnixTimeSeconds(matchRecent.MatchTime).UtcDateTime;

                if (match.HomeTeamLastMatchDate == DateTime.MinValue && match.AwayTeamLastMatchDate == DateTime.MinValue)
                {
                    match.HomeTeamLastMatchDate = GetLastMatchDate(match.HomeTeamId);
                    match.AwayTeamLastMatchDate = GetLastMatchDate(match.AwayTeamId);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return match;
        }

        private async Task MapExistingMatchInfoToLive(LiveMatchModel liveMatch, LiveMatchModel existingMatch)
        {
            try
            {
                if (string.IsNullOrEmpty(existingMatch.HomeTeamId) || string.IsNullOrEmpty(existingMatch.AwayTeamId) || string.IsNullOrEmpty(existingMatch.CompetitionId))
                {
                    await CompleteMatchInfo(liveMatch);
                    return;
                }

                liveMatch.MatchStartTime = liveMatch.MatchStatus == 2 ? liveMatch.CurrentKickoffTime : existingMatch.MatchStartTime;

                liveMatch.CompetitionId = existingMatch.CompetitionId;
                liveMatch.SeasonId = existingMatch.SeasonId;

                liveMatch.HomeTeamId = existingMatch.HomeTeamId;

                liveMatch.AwayTeamId = existingMatch.AwayTeamId;

                liveMatch.HomeTeamRank = existingMatch.HomeTeamRank;
                liveMatch.AwayTeamRank = existingMatch.AwayTeamRank;

                liveMatch.StageName = existingMatch.StageName;
                liveMatch.RoundNumber = existingMatch.RoundNumber;
                liveMatch.GroupNumber = existingMatch.GroupNumber;

                liveMatch.MatchStartTimeOfficial = existingMatch.MatchStartTimeOfficial;
                liveMatch.HomeTeamLastMatchDate = existingMatch.HomeTeamLastMatchDate;
                liveMatch.AwayTeamLastMatchDate = existingMatch.AwayTeamLastMatchDate;

                liveMatch.CompetitionType = existingMatch.CompetitionType;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine(ex);
            }
        }




        private DateTime GetLastMatchDate(string teamId)
        {
            try
            {
                List<RecentMatchModel> matches;
                if (!_inMemoryMatch.GetPastMatchesHash().TryGetValue(teamId, out matches) || matches == null || !matches.Any())
                {
                    return DateTime.MinValue;
                }

                return matches
                    .OrderByDescending(match => match.MatchStartTimeOfficial)
                    .FirstOrDefault()?
                    .MatchStartTimeOfficial ?? DateTime.MinValue;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return DateTime.MinValue;
            }
        }


        private async Task AttachInsightsToMatches(List<LiveMatchDto> matches)
        {
            try
            {
                foreach (LiveMatchDto match in matches)
                {
                    Insights homeTeamInsights = (await _insights.GetAllInsightsOfTeam(match.HomeTeamId)).Items;
                    Insights awayTeamInsights = (await _insights.GetAllInsightsOfTeam(match.AwayTeamId)).Items;

                    if (homeTeamInsights is null || awayTeamInsights is null)
                    {
                        InsightsDto dummy = new()
                        {
                            LiveInsights = new LiveInsightsDto(),
                            HistoricalInsights = new HistoricalInsightsDto()
                        };
                        match.HomeTeamInsights = dummy;
                        match.AwayTeamInsights = dummy;
                        continue;
                    }

                    LiveInsightsDto homeTeamLiveInsights = MappingService.MapToLiveInsightsDto(homeTeamInsights);
                    LiveInsightsDto awayTeamLiveInsights = MappingService.MapToLiveInsightsDto(awayTeamInsights);

                    HistoricalInsightsDto homeTeamHistoricalInsights = MappingService.MapInsightsToHistoricalInsightsDto(homeTeamInsights);

                    HistoricalInsightsDto awayTeamHistoricalInsights = MappingService.MapInsightsToHistoricalInsightsDto(awayTeamInsights);

                    InsightsDto homeTeamInsightsDto = new InsightsDto()
                    {
                        HistoricalInsights = homeTeamHistoricalInsights,
                        LiveInsights = homeTeamLiveInsights
                    };

                    InsightsDto awayTeamInsightsDto = new InsightsDto()
                    {
                        HistoricalInsights = awayTeamHistoricalInsights,
                        LiveInsights = awayTeamLiveInsights
                    };
                    match.HomeTeamInsights = homeTeamInsightsDto;
                    match.AwayTeamInsights = awayTeamInsightsDto;
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return;
            }
        }

        public async Task<bool> RefreshFilters()
        {
            try
            {
                List<LiveMatchDto> liveMatches = _inMemoryMatch.GetAllLiveMatches();
                List<LiveMatchFilterDto> filters = new List<LiveMatchFilterDto>();
                foreach (var liveMatch in liveMatches)
                {
                    LiveMatchFilterDto filter = new LiveMatchFilterDto()
                    {
                        MatchId = liveMatch.MatchId,
                        CompetitionName = liveMatch.CompetitionName,
                        CompetitionType = liveMatch.CompetitionType,
                        CleverLabels = new List<string>()
                    };
                    HashSet<string> uniqueLabels = new HashSet<string>();
                    uniqueLabels.UnionWith(ConvertToCamelCaseFields((await _cleverLabelServices.GetAllLabels(liveMatch.HomeTeamId)).Items));
                    uniqueLabels.UnionWith(ConvertToCamelCaseFields((await _cleverLabelServices.GetAllLabels(liveMatch.AwayTeamId)).Items));
                    filter.CleverLabels = uniqueLabels.ToList();
                    filters.Add(filter);
                }
                _inMemoryMatch.SetLiveFilters(filters);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private List<string> ConvertToCamelCaseFields(CleverLabelsDto dto)
        {
            return dto.GetType()
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                      .Where(p => p.PropertyType == typeof(bool) && (bool)p.GetValue(dto) == true)
                      .Select(p => Char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1))
                      .ToList();
        }

    }
}
