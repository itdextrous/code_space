using AutoMapper;
using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services.FootballServices;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class HistoricalStatsService : IHistoricalStatsService
	{

		private readonly ILogger<HistoricalStatsService> _logger;
		private readonly IHistoricalStatsRepository _historicalStatsRepo;
		private readonly MatchInMemoryService _inMemory;
		private readonly IMapper _mapper;

		public HistoricalStatsService(ILogger<HistoricalStatsService> logger, IHistoricalStatsRepository repo, MatchInMemoryService inMemory, IMapper mapper)
		{
			_logger = logger;
			_historicalStatsRepo = repo;
			_inMemory = inMemory;
			_mapper = mapper;
		}

		public async Task<List<RecentMatchModel>> GetMatches(string teamId)
		{
			try
			{
				//List<RecentMatchModel> matches = _inMemory.GetRecentMatches().Where(rm => rm.HomeTeamId.Equals(teamId) || rm.AwayTeamId.Equals(teamId)).Where(rm => rm.MatchStartTimeOfficial >= DateTime.UtcNow.AddDays(-30)).ToList();
				List<RecentMatchModel> matches = _inMemory.GetPastMatchesHash()[teamId] ?? new List<RecentMatchModel>();
				if (matches is not null) return matches;
				return new List<RecentMatchModel>();
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new List<RecentMatchModel>();
			}
		}

		public async Task<Result<HistoricalStatsResponseMatch>> GetStatsOfMatch(string matchId)
		{
			try
			{
				LiveMatchDto lMatch = _inMemory.GetAllLiveMatches().Where(match => match.MatchId.Equals(matchId)).SingleOrDefault();

				UpcomingMatch uMatch =_inMemory.GetUpcomingMatches().Where(um => um.Id.Equals(matchId)).SingleOrDefault();
				if (lMatch is null && uMatch is null)
				{
					return new Result<HistoricalStatsResponseMatch>(404, false, "No match found with this id", null);
				}

				List<RecentMatchModel> homeMatches = new() ;
				List<RecentMatchModel> awayMatches = new() ;

				List<HistoricalStatsResponseMatch> stats = _inMemory.GetHistoricalStats();

				foreach (var stat in stats)
				{
					if (lMatch is null)
					{
						if (stat.MatchId.Equals(uMatch.Id))
						{
							return new Result<HistoricalStatsResponseMatch>(200, true, "Historical stats", stat);
						}
					}
					else
					{
						if (stat.MatchId.Equals(lMatch.MatchId))
						{
							return new Result<HistoricalStatsResponseMatch>(200, true, "Historical stats", stat);
						}
					}
				}

				HistoricalStatsResponseMatch res = new HistoricalStatsResponseMatch()
				{
					MatchId = matchId,
					HomeTeamStats = await GetTeamStats(lMatch is null ? uMatch.HomeTeam.Id : lMatch.HomeTeamId),
					AwayTeamStats = await GetTeamStats(lMatch is null ? uMatch.AwayTeam.Id : lMatch.AwayTeamId)
				};
				stats.Add(res);
				_inMemory.SetHistoricalStats(stats);
				return new Result<HistoricalStatsResponseMatch>(200, true, "stats", res);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new Result<HistoricalStatsResponseMatch>(500, false, "Internal server error", null );
			}
		}

		//private Result<HistoricalStatsResponseMatch> test(string teamId)
		//{
		//	HistoricalStatsResponseMatch res = new HistoricalStatsResponseMatch()
		//	{
		//		MatchId = "test",
		//		HomeTeamStats = GetTeamStats(teamId)
		//	};
		//	return Result<HistoricalStatsResponseMatch>.Success("",res);
		//}




		public async Task<HistoricalStatsResponseTeam> GetTeamStats(string teamId)
		{
			try
			{
				//List<RecentMatchModel> matchesDb = await _historicalStatsRepo.GetPastMatchesOfTeam(teamId);
				Dictionary<string, List<RecentMatchModel>> pastMatches = _inMemory.GetPastMatchesHash();
				List<RecentMatchModel> matchesDb = pastMatches.ContainsKey(teamId) ? pastMatches[teamId] : new List<RecentMatchModel>();
				List<RecentMatchDto> matches = matchesDb.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
				//List<Histo> pastMatches = new List<HistoricalMatch>();
				foreach(RecentMatchDto match in matches)
				{
					BindMatchToResult(match);
				}

				float n = matches.Count;
				HistoricalStatsResponseTeam stats = new()
				{
					TeamId = teamId,
					pastMatches = matches
				};

				stats.AvgGoalsScoredInterval.Add(0);
				stats.AvgGoalsScoredInterval.Add(0);
				stats.AvgGoalsScoredInterval.Add(0);
				stats.AvgCornersInterval.Add(0);
				stats.AvgCornersInterval.Add(0);
				stats.AvgCornersInterval.Add(0);

				foreach (RecentMatchDto match in matches)
				{
					if (match.HomeTeamId.Equals(teamId))
					{

						List<int> halves = new List<int> { match.HomeTeamHalfTimeScore, match.HomeGoals, 0 };

						stats.GoalMinutes.Add(match.HomeGoalMinutes);
						stats.CornerMinutes.Add(match.HomeCornerMinutes);
						stats.ShotsOnTargetMinutes.Add(match.HomeShotsOnTargetMinutes);

						stats.AvgPossession += (((float)match.HomePossession) / n);

						stats.MatchResult.Add(GetResult(match, match.HomeTeamId));
						// to be checked
						stats.ScoredFirstAndWon.Add(ScoredFirstAndWin(match, match.HomeTeamId));
						stats.AvgShots += ((match.HomeShotsOffTarget + match.HomeShotsOnTarget) / (float)n);

						stats.AvgShotsOnTargetPercentage += (match.HomeShotsOnTarget + match.HomeShotsOffTarget == 0 ? 0 : (((float)(match.HomeShotsOnTarget * 100 / (match.HomeShotsOnTarget + match.HomeShotsOffTarget))) / n));

						stats.AvgAttacks += ((match.HomeAttacks) / (float) n);
						stats.AvgDangerousAttacksPercentage += (match.HomeAttacks == 0 ? 0 : (((float)(match.HomeDangerousAttacks * 100 / match.HomeAttacks)) / n));

						stats.AvgRedCards += ((float)match.HomeRedCards) / n;
						stats.AvgYellowCards += match.HomeYellowCards / (float)n;

						stats.TotalGoals += match.HomeGoals;
						stats.AvgGoalsScored += match.HomeGoals / (float)n;
						stats.AvgGoalsTime += (match.HomeGoals == 0 ? 0 : ((((float)(match.OverTime ? 120 : 90)) / match.HomeGoals) / n));
						stats.AvgGoalsScoredInterval[0] += (((float)match.HomeTeamHalfTimeScore )/ n);
						stats.AvgGoalsScoredInterval[1] += ((float)GoalsCalculationService.GetGoalsScoredInInterval(match.HomeGoalMinutes, halves, 45, 75) / n);
						stats.AvgGoalsScoredInterval[2] += ((float)GoalsCalculationService.GetGoalsScoredInInterval(match.HomeGoalMinutes, halves, 75, 95) / n);

						stats.AvgCorners += match.HomeCorners / (float)n;
						stats.AvgCornersTime += (match.HomeCorners == 0 ? 0 : (((match.OverTime ? 120 : 90) / match.HomeCorners) / n));
						stats.AvgCornersInterval[0] += 0 / n;
						stats.AvgCornersInterval[1] += 0 / n;
						stats.AvgCornersInterval[2] += match.HomeCorners /  n;
						stats.AvgShotsOnTarget += match.HomeShotsOnTarget / n;
						stats.AvgShotConcededMinute += match.AwayShotsOnTarget + match.AwayShotsOffTarget == 0 ? 0 : (90 / (match.AwayShotsOffTarget + match.AwayShotsOnTarget) / (float) n);
						//match.HomePenaltiesRecord.ToCharArray().Take(5).ToList().ForEach(p => stats.Penalties.Add(p == 'g'));
					}
					else
					{
						List<int> halves = new(){ match.AwayTeamHalfTimeScore, match.AwayGoals, 0 };
						stats.GoalMinutes.Add(match.AwayGoalMinutes);
						stats.CornerMinutes.Add(match.AwayCornerMinutes);
						stats.ShotsOnTargetMinutes.Add(match.AwayShotsOnTargetMinutes);
						stats.AvgPossession += (match.AwayPossession / n);
						// to be checked

						//string res = match.AwayTeamComebackToWin ? "cw" : match.AwayTeamComebackToDraw ? "cd" : !match.HomeWin ? "w" : match.GameDrawn ? "d" : "l";

						stats.MatchResult.Add(GetResult(match, match.AwayTeamId));


						// to be checked

						stats.ScoredFirstAndWon.Add(ScoredFirstAndWin(match, match.AwayTeamId));

						stats.AvgShots += ((match.AwayShotsOffTarget + match.AwayShotsOnTarget) / n);

						stats.AvgShotsOnTargetPercentage += (match.AwayShotsOnTarget + match.AwayShotsOffTarget == 0 ? 0 : (((match.AwayShotsOnTarget / (match.AwayShotsOnTarget + match.AwayShotsOffTarget)) * 100) / n));

						stats.AvgAttacks += (match.AwayAttacks == 0 ? 0 : ((match.AwayAttacks) / n));

						stats.AvgDangerousAttacksPercentage += (match.AwayAttacks == 0 ? 0 : (((match.AwayDangerousAttacks / match.AwayAttacks) * 100) / n));

						stats.AvgRedCards += match.AwayRedCards / (float)n;
						stats.AvgYellowCards += match.AwayYellowCards / (float)n;

						stats.TotalGoals += match.AwayGoals;
						stats.AvgGoalsScored += match.AwayGoals / (float)n;
						stats.AvgGoalsTime += (match.AwayGoals == 0 ? 0 : (((match.OverTime ? 120 : 90) / match.AwayGoals) / n));
						stats.AvgGoalsScoredInterval[0] += (match.AwayTeamHalfTimeScore / n);
						stats.AvgGoalsScoredInterval[1] += ((float)GoalsCalculationService.GetGoalsScoredInInterval(match.AwayGoalMinutes, halves, 45, 75) / n);
						stats.AvgGoalsScoredInterval[2] += ((float)GoalsCalculationService.GetGoalsScoredInInterval(match.AwayGoalMinutes, halves, 75, 95) / n);

						stats.AvgCorners += match.AwayCorners / (float)n;
						stats.AvgCornersTime += (match.AwayCorners == 0 ? 0 : (((match.OverTime ? 120 : 90) / match.AwayCorners) / n));
						stats.AvgCornersInterval[0] += 0 / n;
						stats.AvgCornersInterval[1] += 0 / n;
						stats.AvgCornersInterval[2] += match.AwayCorners / n;

						stats.AvgShotsOnTarget += match.AwayShotsOnTarget / n;
						stats.AvgShotConcededMinute += match.HomeShotsOnTarget + match.HomeShotsOffTarget == 0 ? 0 : (90 / (match.HomeShotsOffTarget + match.HomeShotsOnTarget) / n);
						//match.AwayPenaltiesRecord.ToCharArray().Take(5).ToList().ForEach(p => stats.Penalties.Add(p == 'g'));
					}
				}
				return stats;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}

		}



		private bool ScoredFirstAndWin(RecentMatchDto match, string teamId)
		{
			try
			{
				if (match.HomeTeamId.Equals(teamId))
				{
					if (!match.HomeWin) return false;
				}
				else
				{
					if (match.HomeWin || match.GameDrawn) return false;
				}

				string homeGoalMinutes = match.HomeGoalMinutes;
				string awayGoalMinutes = match.AwayGoalMinutes;
				List<int> homeHalves = new List<int>() { match.HomeTeamHalfTimeScore, match.HomeGoals, 0 };
				List<int> awayHalves = new List<int>() { match.AwayTeamHalfTimeScore, match.AwayGoals, 0 };

				int homeFirstGoal = GoalsCalculationService.GetFirstGoalMinute(homeGoalMinutes, homeHalves);
				int awayFirstGoal = GoalsCalculationService.GetFirstGoalMinute(awayGoalMinutes, awayHalves);

				if (match.HomeTeamId.Equals(teamId))
					return awayFirstGoal < 0 || homeFirstGoal < awayFirstGoal;
				else
					return homeFirstGoal < 0 || awayFirstGoal < homeFirstGoal;
		
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}



		private string GetResult(RecentMatchDto match, string teamId)
		{
			try
			{
				MatchResult res = GoalsCalculationService.GetMatchResult(match);
                if (match.HomeTeamId.Equals(teamId))
				{
					if (res.AwayTeamWin) return "l";
					if (res.HomeTeamComeBackToDraw) return "cd";
					if (res.Draw) return "d";
					if (res.HomeTeamWin && res.ComeBackToWin) return "cw";
					return "w";
				}
				else
				{
					if (res.HomeTeamWin) return "l";
					if (res.AwayTeamComeBackToDraw) return "cd";
					if (res.Draw) return "d";
					if (res.AwayTeamWin && res.ComeBackToWin) return "cw";
					return "w";
				}
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}


		private void BindMatchToResult(RecentMatchDto match)
		{
			try
			{
				MatchResult res = GoalsCalculationService.GetMatchResult(match);
				match.HomeTeamComebackToDraw = res.HomeTeamComeBackToDraw;
				match.AwayTeamComebackToDraw = res.AwayTeamComeBackToDraw;
				match.HomeTeamComebackToWin = res.HomeTeamWin && res.ComeBackToWin;
				match.AwayTeamComebackToWin = res.AwayTeamWin && res.ComeBackToWin;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
		}


	}
}
