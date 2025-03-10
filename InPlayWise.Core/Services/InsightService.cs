using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services.FootballServices;
using InPlayWise.Data.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InPlayWise.Core.Services
{
    public class InsightService : IInsightService
    {
        private readonly IInsightsRepository _insightsRepo;
        private readonly ILogger<InsightService> _logger;
        private readonly IHttpContextService _httpContext;
        private readonly MatchInMemoryService _inMemory;
        int n;

        public InsightService(IInsightsRepository insightsRepository, ILogger<InsightService> logger, IHttpContextService httpContext, MatchInMemoryService matchInMemoryService ) {
            _insightsRepo = insightsRepository;
            _logger = logger;
            _httpContext = httpContext;
            _inMemory = matchInMemoryService;
            n = 10;
        }


		public async Task<Result<Insights>> GetAllInsightsOfTeam(string teamId)
		{
			try
			{
                Insights res = new Insights();
				List<Insights> insights = _inMemory.GetAllInsights();
                List<LiveMatchDto> matches = _inMemory.GetAllLiveMatches();

				foreach (Insights ins in insights)
				{
                    if (ins is not null && ins.TeamId is not null && ins.TeamId.Equals(teamId))
                    {
                        if(ins.UpdatedTime < DateTime.UtcNow.AddSeconds(-10))
                        {
							foreach (var match in matches)
							{
                                if(string.IsNullOrEmpty(match.HomeTeamId) || string.IsNullOrEmpty(match.AwayTeamId))
                                {
									return new Result<Insights>(500, false, "Internal Server Error", null);
								}
								if (match.HomeTeamId.Equals(teamId))
                                {
									MapLiveInsights(ins, match, match.HomeTeamId);
                                    break;
								}
                                else if (match.AwayTeamId.Equals(teamId))
								{
									MapLiveInsights(ins, match, match.AwayTeamId);
                                    break;
								}
							}
						}
						return new Result<Insights>(200, true, "insights", ins);
					}
				}
				res = await GetHistoricalInsightsOfTeam(teamId);
				insights.Add(res);
                _inMemory.SetInsights(insights);
				return new Result<Insights>(200, true, "insights", res);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new Result<Insights>(500, false, "Internal Server Error", null);
			}
		}

        private bool MapLiveInsights(Insights insights, LiveMatchDto match, string teamId)
        {
            try
            {
                insights.MatchId = match.MatchId;
                if (match.HomeTeamId.Equals(teamId))
                {
					insights.LiveCorners = (int)match.HomeCorners;
					insights.LivePossession = (int)match.HomePossession;
					insights.LiveDangerousAttackPercentage = (int)(match.HomeDangerousAttacks == 0 ? 0 : (match.HomeDangerousAttacks * 100 / (match.HomeAttacks + match.HomeDangerousAttacks)));
					insights.LiveShotsOnTargetPercentage = (int) (match.HomeShotsOffTarget + match.HomeShotsOnTarget == 0 ? 0 : (match.HomeShotsOnTarget * 100 / (match.HomeShotsOnTarget + match.HomeShotsOffTarget)));
				}
                else
                {
					insights.LiveCorners = (int)match.AwayCorners;
					insights.LivePossession = (int) match.AwayPossession;
					insights.LiveDangerousAttackPercentage = (int) (match.AwayDangerousAttacks == 0 ? 0 : (match.AwayDangerousAttacks * 100 / (match.AwayAttacks + match.AwayDangerousAttacks)));
					insights.LiveShotsOnTargetPercentage = (int) (match.AwayShotsOffTarget + match.AwayShotsOnTarget == 0 ? 0 : (match.AwayShotsOnTarget * 100 / (match.AwayShotsOnTarget + match.AwayShotsOffTarget)));
				}
                insights.MatchId = match.MatchId;
                insights.UpdatedTime = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        private async Task<Insights> GetHistoricalInsightsOfTeam(string teamId)
        {
            try
            {
                //List<RecentMatchModel> matches = await _insightsRepo.GetLastNMatchesOfTeam(teamId, n);

                //List<RecentMatchModel> matchesAll = _inMemory.GetRecentMatches().Where(m => ((m.HomeTeamId.Equals(teamId)) ||(m.AwayTeamId.Equals(teamId)))).OrderByDescending(match => match.MatchStartTimeOfficial).ToList();
                List<RecentMatchModel> matchesAll;
                if (!_inMemory.GetPastMatchesHash().TryGetValue(teamId, out matchesAll))
                {
                    matchesAll = new List<RecentMatchModel>();
                }
                List<RecentMatchModel> matches = matchesAll.Take(10).ToList();

				return new Insights()
				{
					TeamId = teamId,
					GoalsScoredAvg = GoalsScoredAvgByTeam(matches, teamId),
					GoalsScoredFirstHalfAvg = GoalsScoredFirstHalfAvg(matches, teamId),
					GoalsScoredSecondHalfAvg = GoalsScoredSecondHalfAvg(matches, teamId),
					GoalsConcededAvg = GoalsConcededAvg(matches, teamId),
					GoalsConcededFirstHalfAvg = GoalsConcededFirstHalfAvg(matches, teamId),
					GoalsConcededSecondHalfAvg = GoalsConcededSecondHalfAvg(matches, teamId),
					ScoredFirstHalfAndSecondHalfPercent = ScoredFirstHalfAndSecondHalfPercent(matches, teamId),
					GoalsConcededFirstHalfAndSecondHalfPercent = GoalsConcededFirstHalfAndSecondHalfPercent(matches, teamId),
					GoalsAvg = GoalsAvg(matches),
					GoalsFirstHalfAvg = GoalsFirstHalfAvg(matches, teamId),
					GoalsSecondHalfAvg = GoalsSecondHalfAvg(matches, teamId),
					GoalsFirstHalfAndSecondHalfPercent = GoalsFirstHalfAndSecondHalfPercent(matches, teamId),
					OverZeroPointFivePercent = OverZeroPointFiveGoalsPercent(matches),
					OverOnePointFivePercent = OverOnePointFiveGoalsPercent(matches),
					OverTwoPointFivePercent = OverTwoPointFiveGoalsPercent(matches),
					OverThreePointFivePercent = OverThreePointFiveGoals(matches),
					BothTeamsScoredPercent = BothTeamsScoredPercent(matches),
					NoGoalScoredPercent = NoGoalScoredPercent(matches),
					HomeWinPercent = HomeWinPercent(matches, teamId),
					AwayWinPercent = AwayWinPercent(matches, teamId),
					ShotsOnTarget = ShotsOnTarget(matches, teamId),
					DangerousAttack = DangerousAttack(matches, teamId),
					ShotsOnTargetAverage = ShotsOnTargetAverage(matches, teamId),
					DangerousAttacksAverage = DangerousAttacksAverage(matches, teamId),
					AverageCornersOfTeam = AverageCornersOfTeam(matches, teamId),
					AverageCornersInGame = AverageCornersInGame(matches),
					CleanSheetPercent = CleanSheetPercent(matches, teamId),
				};
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }


        public async Task<Result<MatchInsightsResponseDto>> GetInsightsOfMatch(string matchId)
        {
            try
            {
                //if(! await ValidateHit(matchId))
                //{
                //    return new Result<MatchInsightsResponseDto>(400, false, "Your limit has expired", null);
                //}

                LiveMatchDto match = null;
                MatchInsightsResponseDto insightsOfMatch = new MatchInsightsResponseDto();
                foreach(LiveMatchDto m in _inMemory.GetAllLiveMatches())
                {
                    if (m.MatchId.Equals(matchId))
                        match = m;
                }
                foreach (Insights ins in _inMemory.GetAllInsights())
                {
                    if (ins.TeamId.Equals(match.HomeTeamId))
                        insightsOfMatch.HomeInsights = ins;
                    if (ins.TeamId.Equals(match.AwayTeamId))
                        insightsOfMatch.AwayInsights = ins;
                }
                return new Result<MatchInsightsResponseDto>(200, true, "",insightsOfMatch);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<MatchInsightsResponseDto>(500, false, "Internal server error", new MatchInsightsResponseDto());
            }
        }



		private float GoalsScoredAvgByTeam(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0 ;
                foreach (RecentMatchModel match in matches)
                    sum += match.HomeTeamId?.Equals(teamId) == true ? match.HomeGoals : match.AwayGoals;
                return sum / matches.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsScoredFirstHalfAvg(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
				if (matches is null || matches.Count == 0)
					return 0;
				float sum = 0;
                List<int> goalMinutes ;
                foreach (RecentMatchModel match in matches)
                {
                    goalMinutes = new List<int>();
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        goalMinutes = GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes);
                        sum += goalMinutes.Count;
                    }
                    else
                    {
                        goalMinutes = GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes);
                        sum += goalMinutes.Count;
                    }
                }
                return sum / matches.Count; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsScoredSecondHalfAvg(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                List<int> goalMinutes;
                foreach (RecentMatchModel match in matches)
                {
                    goalMinutes = new List<int>();
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        goalMinutes = GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes);
                        sum += goalMinutes.Count;
                    }
                    else
                    {
                        goalMinutes = GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes);
                        sum += goalMinutes.Count;
                    }
                }
                return sum / matches.Count; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsConcededAvg(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        sum += match.AwayGoals;
                    }
                    else
                    {
                        sum += match.HomeGoals;
                    }
                }
                return sum / matches.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsConcededFirstHalfAvg(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                List<int> opponentGoalMinutes;
                foreach (RecentMatchModel match in matches)
                {
                    opponentGoalMinutes = new List<int>();
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        opponentGoalMinutes = GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes);
                        sum += opponentGoalMinutes.Count;
                    }
                    else
                    {
                        opponentGoalMinutes = GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes);
                        sum += opponentGoalMinutes.Count;
                    }
                }
                return sum / matches.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }


        private float GoalsConcededSecondHalfAvg( List<RecentMatchModel> matches ,string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                List<int> opponentGoalMinutes;
                foreach (RecentMatchModel match in matches)
                {
                    opponentGoalMinutes = new List<int>();
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        opponentGoalMinutes = GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes);
                        sum += opponentGoalMinutes.Count;
                    }
                    else
                    {
                        opponentGoalMinutes = GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes);
                        sum += opponentGoalMinutes.Count;
                    }
                }
                return sum / n;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }


        private int GoalsConcededFirstHalfAndSecondHalfPercent(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                List<int> opponentGoalMinutesFirstHalf;
                List<int> opponentGoalMinutesSecondHalf;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    opponentGoalMinutesFirstHalf = new List<int>();
                    opponentGoalMinutesSecondHalf = new List<int>();
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        opponentGoalMinutesFirstHalf = GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes);
                        opponentGoalMinutesSecondHalf = GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes);
                        if(opponentGoalMinutesFirstHalf.Count > 0 && opponentGoalMinutesSecondHalf.Count > 0)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        opponentGoalMinutesFirstHalf = GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes);
                        opponentGoalMinutesSecondHalf = GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes);
                        if (opponentGoalMinutesFirstHalf.Count > 0 && opponentGoalMinutesSecondHalf.Count > 0)
                        {
                            count++;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }


        private int ScoredFirstHalfAndSecondHalfPercent(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                List<int> goalMinutesFirstHalf;
                List<int> goalMinutesSecondHalf;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    goalMinutesFirstHalf = new List<int>();
                    goalMinutesSecondHalf = new List<int>();
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        goalMinutesFirstHalf = GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes);
                        goalMinutesSecondHalf = GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes);
                        if (goalMinutesFirstHalf.Count > 0 && goalMinutesSecondHalf.Count > 0) count++;
                    }
                    else
                    {
                        goalMinutesFirstHalf = GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes);
                        goalMinutesSecondHalf = GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes);
                        if (goalMinutesFirstHalf.Count > 0 && goalMinutesSecondHalf.Count > 0)
                        {
                            count++;
                        }
                    }
                }
                return ( (count * 100) / n);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsAvg(List<RecentMatchModel> matches)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                foreach (RecentMatchModel match in matches)
                {
                    sum += match.HomeGoals + match.AwayGoals;
                }
                return sum / n;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsFirstHalfAvg( List<RecentMatchModel> matches ,string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                foreach (RecentMatchModel match in matches)
                {
                    sum += GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes).Count + GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes).Count; 
                }
                return sum / n;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float GoalsSecondHalfAvg(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float sum = 0;
                foreach (RecentMatchModel match in matches)
                {
                    sum += GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes).Count + GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes).Count;
                }
                return sum / n;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }


        private int GoalsFirstHalfAndSecondHalfPercent(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    bool scoredInBoth = false;
                    scoredInBoth = (GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes).Count + GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes).Count > 0) && (GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes).Count + GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes).Count > 0) ;
                    if (scoredInBoth)
                        count++;
                }
                return (count * 100) / n;

			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private int OverZeroPointFiveGoalsPercent(List<RecentMatchModel> matches)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int count = 0;
                foreach(RecentMatchModel match in matches)
                {
                    if (match.HomeGoals + match.AwayGoals >= 1)
                        count++;
                }
                return (count * 100 / matches.Count);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }



		private int OverOnePointFiveGoalsPercent(List<RecentMatchModel> matches)
		{
			try
			{
				if (matches is null || matches.Count == 0)
					return 0;
				int count = 0;
				foreach (RecentMatchModel match in matches)
				{
					if (match.HomeGoals + match.AwayGoals >= 2)
						count++;
				}
				return (count * 100 / matches.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return 0;
			}
		}



		private int OverTwoPointFiveGoalsPercent(List<RecentMatchModel> matches)
		{
			try
			{
				if (matches is null || matches.Count == 0)
					return 0;
				int count = 0;
				foreach (RecentMatchModel match in matches)
				{
					if (match.HomeGoals + match.AwayGoals >= 3)
						count++;
				}
				return (count * 100 / matches.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return 0;
			}
		}



		private int OverThreePointFiveGoals(List<RecentMatchModel> matches)
		{
			try
			{
				if (matches is null || matches.Count == 0)
					return 0;
				int count = 0;
				foreach (RecentMatchModel match in matches)
				{
					if (match.HomeGoals + match.AwayGoals >= 4)
						count++;
				}
				return (count * 100 / matches.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return 0;
			}
		}



		private int BothTeamsScoredPercent(List<RecentMatchModel> matches)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    bool bothTeamsScored = false;
                    bothTeamsScored = match.HomeGoals > 0 && match.AwayGoals > 0;
                    if(bothTeamsScored)
                        count++;
                }
                return (int)((count * 100) / n);

			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private int NoGoalScoredPercent( List<RecentMatchModel> matches)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    bool noTeamScored = false;
                    noTeamScored = match.HomeGoals == 0 && match.AwayGoals == 0;
                    if (noTeamScored)
                        count++;
                }
                return (count * 100) / n;

			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private int HomeWinPercent(List<RecentMatchModel> matchesAll, string teamId)
        {
            try
            {
                List<RecentMatchModel> matches = new();
                foreach(var match in matchesAll)
                {
                    if (match.HomeTeamId.Equals(teamId))
                        matches.Add(match);
                }
                if ( matches.Count == 0)
                    return 0;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeWin)
                        count++;
                }
                return (int)((count * 100) / matches.Count);
			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private int AwayWinPercent(List<RecentMatchModel> matchesAll, string teamId)
        {
            try
            {
                List<RecentMatchModel> matches = new();

                foreach(RecentMatchModel match in matchesAll)
                {
                    if(match.AwayTeamId.Equals(teamId))
                        matches.Add(match);
                }
                if ( matches.Count == 0 )
                    return 0;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (!match.HomeWin)
                        count++;
                }
                return (int)((count * 100) / matches.Count);
			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private int ShotsOnTarget(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int shotsOnTarget = 0;
                int shotsOffTarget = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        shotsOffTarget += match.HomeShotsOffTarget;
                        shotsOnTarget += match.HomeShotsOnTarget;
                    }
                    else
                    {
                        shotsOnTarget += match.AwayShotsOnTarget;
                        shotsOffTarget += match.AwayShotsOffTarget;
                    }
                }
                return (shotsOffTarget == 0 ? shotsOnTarget : (shotsOnTarget * 100 / (shotsOnTarget + shotsOffTarget))) ;

			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private int DangerousAttack(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int totalAttacks = 0;
                int dangerousAttacks = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        totalAttacks += (match.HomeAttacks + match.HomeDangerousAttacks);
                        dangerousAttacks += (match.HomeDangerousAttacks);
                    }
                    else
                    {
                        totalAttacks += (match.AwayAttacks + match.AwayDangerousAttacks);
                        dangerousAttacks += (match.AwayDangerousAttacks);
                    }
                }
                var res = (totalAttacks == 0 ? 0 : ((dangerousAttacks * 100 / totalAttacks)));
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float ShotsOnTargetAverage(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float shotsOnTarget = 0;

                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                        shotsOnTarget += match.HomeShotsOnTarget;

                    else
                        shotsOnTarget += match.AwayShotsOnTarget;

                }
                return shotsOnTarget / matches.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float DangerousAttacksAverage(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int dangerousAttacks = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                        dangerousAttacks += match.HomeDangerousAttacks;
                    else
                        dangerousAttacks += match.AwayDangerousAttacks;
                }
                return dangerousAttacks / n;
			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float AverageCornersOfTeam(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float corners = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                        corners += match.HomeCorners;
                    else
                        corners += match.AwayCorners;
                }
                return corners / matches.Count;

			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        private float AverageCornersInGame(List<RecentMatchModel> matches)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                float corners = 0;
                foreach (RecentMatchModel match in matches)
                {
                    corners += match.HomeCorners + match.AwayCorners;
                }
                return corners / n;
			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }


        private int CleanSheetPercent(List<RecentMatchModel> matches,  string teamId)
        {
            try
            {
                if (matches is null || matches.Count == 0)
                    return 0;
                int count = 0;
                foreach (RecentMatchModel match in matches)
                {
                    if (match.HomeTeamId?.Equals(teamId) == true)
                    {
                        if (match.AwayGoals == 0)
                            count++;
                    }
                    else
                    {
                        if (match.HomeGoals == 0)
                            count++;
                    }
                }
                return (int)((count * 100) / n) ;
			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

		private async Task<bool> ValidateHit(string matchId)
		{
			try
			{
				string userId = _httpContext.GetUserId();
				PlanFeatures planFeatures = await _insightsRepo.GetPlanFeatures(userId);
                LiveInsightsPerGame existingHits = await _insightsRepo.GetUserHitsOnMatch(userId, matchId);
                if (existingHits is null || planFeatures is null) return false;
                if(existingHits.Hits == -1){
                    existingHits.MatchId = matchId;
                    existingHits.UserId = userId;
                    existingHits.Hits = 1;
                    bool added = await _insightsRepo.AddCounter(existingHits);
                    return added;
                }
                if (existingHits.Hits >= planFeatures.LiveInsightPerGame) return false;
                existingHits.Hits ++ ;
                bool updated = await _insightsRepo.UpdateCounter(existingHits);
                return updated;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

	}
}
