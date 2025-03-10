using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services.FootballServices;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class CleverLabelServices : ICleverLabelServices
    {
        private readonly ICleverLabelsRepository _clRepo;
        private readonly ILogger<CleverLabelServices> _logger;
        private readonly IHttpContextService _context;
        private readonly MatchInMemoryService _inMemory;
        

        public CleverLabelServices(ICleverLabelsRepository cleverLabelsRepo, ILogger<CleverLabelServices> logger, IHttpContextService context, MatchInMemoryService mem)
        {
            _clRepo = cleverLabelsRepo;
            _logger = logger;           
            _context = context;
            _inMemory = mem;
        }


        public async Task<Result<CleverLabelsDto>> GetAllLabels(string teamId)
        {
            try
            {
                //if (!await ValidateHit(teamId))
                //    return new Result<CleverLabelsDto>(400, false, "Limit expired", null);

                List<CleverLabelsDto> labels = _inMemory.GetCleverLabels();

                foreach(var lab in labels)
                {
                    if (lab.TeamId.Equals(teamId))
                    {
                        return Result<CleverLabelsDto>.Success("",lab);
                    }
                }
                CleverLabelsDto label = await GetLabelFromDb(teamId);
                labels.Add(label);
                _inMemory.SetCleverLabels(labels);
                return new Result<CleverLabelsDto>
                {
                    Items = label,
                    IsSuccess = true,
                    StatusCode = 200,
                    Message =  "Clever labels"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<CleverLabelsDto>
                {
                    Items =  new CleverLabelsDto(),
                    IsSuccess = true,
                    Message = "Clever labels",
                    StatusCode = 200
                };
            }
        }


        private async Task<CleverLabelsDto> GetLabelFromDb(string teamId)
        {
            try
            {
                
                List<LiveMatchDto> liveMatches = _inMemory.GetAllLiveMatches();
                //List<RecentMatchModel> pastMatches = await _clRepo.GetPastMatchesOfTeam(teamId);

                //List<RecentMatchModel> pastMatches = _inMemory.GetRecentMatches().Where(rm => rm.HomeTeamId.Equals(teamId) || rm.AwayTeamId.Equals(teamId)).ToList();
                List<RecentMatchModel> pastMatches = new List<RecentMatchModel>();
                if (_inMemory.GetPastMatchesHash().ContainsKey(teamId))
                {
                    pastMatches = _inMemory.GetPastMatchesHash()[teamId];
                }
                //List<RecentMatchModel> pastMatches = _inMemory.GetPastMatchesHash()[teamId] ?? new List<RecentMatchModel>();
				List<RecentMatchModel> matches = pastMatches.Take(3).ToList();
				string seasonId = "";
				foreach (var m in liveMatches)
				{
					if (m.HomeTeamId.Equals(teamId) || m.AwayTeamId.Equals(teamId))
					{
						seasonId = m.SeasonId; break;
					}
				}
                CleverLabelsDto labels = new CleverLabelsDto
                {
                    TeamId = teamId,
                    PowerHouse = PowerHouse(matches, teamId),
                    DefensivePowerHouse = await DefensivePowerHouse(teamId, seasonId),
                    HomeDominators = HomeDominators(pastMatches, seasonId, teamId),
                    AwaySpecialists = AwaySpecialists(pastMatches, seasonId, teamId),
                    ScoringSpree = ScoringSpree(matches, teamId),
                    QuickStrikes = QuickStrikes(matches, teamId),
                    EarlyScorers = EarlyScorers(matches, teamId),
                    LateSurge = LateSurge(matches, teamId),
                    SecondHalfSpecialists = SecondHalfSpecialists(matches, teamId),
                    Inconsistent = InConsistent(matches, teamId),
                    UnderDogs = UnderDogs(),
                    EnoughMatches = pastMatches.Count >= 3
                };
                return labels;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }


        private bool PowerHouse(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count < 3) return false;
                
                int matchCount = 0;
                int winCount = 0;
                int drawCount = 0;
                foreach (var match in matches)
                {
                    if (match.GameDrawn) drawCount++;
                    
                    else if (match.HomeTeamId.Equals(teamId) && match.HomeWin) winCount++;
                    
                    else if (match.AwayTeamId.Equals(teamId) && !match.HomeWin) winCount++;
                    
                    matchCount++;
                }
                return (winCount == 3) || (winCount == 2 && drawCount == 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        private bool ScoringSpree(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count < 3) return false;
                int count = 0;
                foreach (var match in matches)
                {
                    if (match.HomeTeamId.Equals(teamId))
                    {
                        if (match.HomeGoals > 2)
                            count++;
                    }
                    else
                    {
                        if (match.AwayGoals > 2)
                            count++;
                    }
                }
                return count >= 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        private bool QuickStrikes(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count < 3) return false;
                int count = 0;
                foreach (var match in matches)
                {
                    if (match.HomeTeamId.Equals(teamId))
                    {
                        int numOfGoals = GoalsCalculationService.GetNumOfGoalsInInterval(0, 10, match.HomeGoalMinutes);
                        if(numOfGoals >= 2) count++;
                    }
                    else
                    {
                        int numOfGoals = GoalsCalculationService.GetNumOfGoalsInInterval(0, 10, match.AwayGoalMinutes);
                        if (numOfGoals >= 2) count++;
                    }
                }
                return count >= 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        private bool EarlyScorers(List<RecentMatchModel> matches, string teamId)
        {

            try
            {
                if (matches is null || matches.Count < 3) return false;
                int count = 0;
                foreach (var match in matches)
                {
                    if (match.HomeTeamId.Equals(teamId))
                    {
                        int numOfGoals = GoalsCalculationService.GetNumOfGoalsInInterval(0, 15, match.HomeGoalMinutes);

                        if (numOfGoals >= 1) count++;
                        
                    }
                    else
                    {
                        int numOfGoals = GoalsCalculationService.GetNumOfGoalsInInterval(0, 15, match.HomeGoalMinutes);

                        if (numOfGoals >= 1) count++;
                    }
                }
                return count >= 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }




        private bool LateSurge(List<RecentMatchModel> matches, string teamId)
        {

            try
            {

                if (matches is null || matches.Count < 3) return false;
                int count = 0;
                foreach (var match in matches)
                {
                    if (match.HomeTeamId.Equals(teamId))
                    {
                        int numOfGoals = GoalsCalculationService.GetNumOfGoalsInIntervalInSecondHalf(80, 150, match.HomeGoalMinutes);
                        if (numOfGoals >= 1) count++;
                    }
                    else
                    {
                        int numOfGoals = GoalsCalculationService.GetNumOfGoalsInIntervalInSecondHalf(80, 150, match.AwayGoalMinutes);
                        if (numOfGoals >= 1) count++;
                    }
                }
                return count >= 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool SecondHalfSpecialists(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count < 3) return false;
                foreach (var match in matches)
                {
                    if (match.HomeTeamId.Equals(teamId))
                    {
                        List<int> goalsFirstHalf = GoalsCalculationService.GoalsFirstHalf(match.HomeGoalMinutes);
                        List<int> goalsSecondHalf = GoalsCalculationService.GoalsSecondHalf(match.HomeGoalMinutes);
                        if (goalsFirstHalf.Count >= goalsSecondHalf.Count) return false ;
                    }
                    else
                    {
                        List<int> goalsFirstHalf = GoalsCalculationService.GoalsFirstHalf(match.AwayGoalMinutes);
                        List<int> goalsSecondHalf = GoalsCalculationService.GoalsSecondHalf(match.AwayGoalMinutes);
                        if (goalsFirstHalf.Count < goalsSecondHalf.Count) return false ;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }




        private bool InConsistent(List<RecentMatchModel> matches, string teamId)
        {
            try
            {
                if (matches is null || matches.Count < 3) return false;
                int drawCount = 0;
                int noGoalCount = 0;
                foreach (var match in matches)
                {
                    if (match.GameDrawn)
                        drawCount++;

                    if (match.HomeTeamId.Equals(teamId))
                    {
                        if (match.HomeGoals == 0)
                            noGoalCount++;
                    }
                    else
                    {
                        if (match.AwayGoals == 0)
                            noGoalCount++;
                    }
                }
                return drawCount >= 2 || noGoalCount >= 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool HomeDominators(List<RecentMatchModel> matches, string seasonId, string teamId)
        {
            try
            {
				if (matches is null || matches.Count < 3) return false;
                int matchCount = 0;
                int wonCount = 0;
                foreach(var match in matches)
                {
                    if (match.SeasonId.Equals(seasonId))
                    {
                        if (match.HomeTeamId.Equals(teamId))
                        {
                            if(match.HomeWin)
                                wonCount++;
                        }
                        matchCount++;
                    }
                }
                return matchCount == 0 ? false : (wonCount/matchCount)*100 >= 75;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

		private bool AwaySpecialists(List<RecentMatchModel> matches, string seasonId, string teamId)
		{
			try
			{
				if (matches is null || matches.Count < 3) return false;
				int matchCount = 0;
				int wonCount = 0;
				foreach (var match in matches)
				{
					if (match.SeasonId.Equals(seasonId))
					{
						if (match.AwayTeamId.Equals(teamId))
						{
							if (!match.HomeWin && !match.GameDrawn)
								wonCount++;
						}
						matchCount++;
					}
				}
				return matchCount == 0 ? false : (wonCount * 100 / matchCount)  >= 60;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}



        private async Task<bool> DefensivePowerHouse(string teamId, string seasonId)
        {
            try
            {
                if(string.IsNullOrEmpty(seasonId))
                    return false;

                //List<RecentMatchModel> matches = await _clRepo.GetAllMatchesOfSeason(seasonId);
                //if (matches is null) return false;

                //Dictionary<string, List<RecentMatchModel>> arranged = new Dictionary<string, List<RecentMatchModel>>();
                //foreach (var match in matches)
                {
      //              foreach(var match2 in matches)
      //              {
      //                  if(match.HomeTeamId.Equals(match2.HomeTeamId) || match.HomeTeamId.Equals(match2.AwayTeamId))
      //                  {
      //                      if (arranged.ContainsKey(match.HomeTeamId))
      //                      {
      //                          arranged[match.HomeTeamId].Add(match2);
      //                      }
      //                      else
      //                      {
      //                          arranged.Add(match.HomeTeamId, new List<RecentMatchModel>() { match2 });
      //                      }
      //                  }
      //                  else if(match.AwayTeamId.Equals(match2.HomeTeamId) || match.AwayTeamId.Equals(match.AwayTeamId))
      //                  {
      //                      if (arranged.ContainsKey(match.AwayTeamId))
      //                      {
      //                          arranged[match.AwayTeamId].Add(match2);
      //                      }
						//	else
						//	{
						//		arranged.Add(match.AwayTeamId, new List<RecentMatchModel>() {match2});
						//	}
						//}
      //              }

                }


                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }


        private bool UnderDogs()
        {
            // odds api not available
            return false;
        }


		//private async Task<bool> ValidateHit(string teamId)
  //      {
  //          try
  //          {
  //              string userId = _context.GetUserId();
  //              PlanFeatures planFeatures = await _clRepo.GetPlanFeatures(userId);
  //              List<CleverLabelsCounter> existingHits = await _clRepo.GetUserHitsOnMatches(userId);
  //              if (existingHits is null || planFeatures is null) return false;

  //              if (existingHits.Count >= planFeatures.CleverLabelling) return false;


  //              foreach(CleverLabelsCounter hit in existingHits)
  //              {
  //                  if (hit.TeamId.Equals(teamId)) return true;
  //              }

  //              CleverLabelsCounter counter = new CleverLabelsCounter()
  //              {
  //                  TeamId = teamId,
  //                  UserId = userId
  //              };

  //              bool added = await _clRepo.AddCounter(counter);
  //              return added;
  //          }
  //          catch (Exception ex)
  //          {
  //              _logger.LogError(ex.ToString());
  //              return false;
  //          }
  //      }
    }
}
