using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace InPlayWise.Core.BackgroundProcess
{
    public class HistoricalMatchBackgroundProcess : IHistoricMatchBackgroundProcess
	{
		private readonly IRecentMatchRepository _recentMatchRepo;
		private readonly ILogger<RecentMatchesService> _logger;
		private readonly MatchInMemoryService _inMemory;
		private readonly IBasicDataServices _basicDataServices;

		public HistoricalMatchBackgroundProcess(IRecentMatchRepository matchRepo, ILogger<RecentMatchesService> logger, MatchInMemoryService inMemory, IBasicDataServices basicDataServices, IConfiguration configuration)
		{
			_recentMatchRepo = matchRepo;
			_logger = logger;
			_inMemory = inMemory;
			_basicDataServices = basicDataServices;
		}

		public async Task<bool> CompleteMatchesInfo()
		{
			try
			{
				List<ApiRecentMatch> apiMatches = (await _basicDataServices.MatchRecent()).Where(rm => rm.MatchTime > DateTimeOffset.UtcNow.AddDays(-3).ToUnixTimeSeconds() && rm.StatusId == 8).ToList();

				apiMatches = apiMatches.OrderBy(match => match.Id).ToList();
				List<string> latestIds = apiMatches.Select(am => am.Id).ToList();

				List<RecentMatchModel> dbMatches = await _recentMatchRepo.GetMatchesWithId(latestIds);

				List<RecentMatchModel> matchesToAdd = new List<RecentMatchModel>();

				foreach (var apiMatch in apiMatches)
				{
					string matchId = apiMatch.Id;
					bool found = false;
					foreach (var dbMatch in dbMatches)
					{
						if (dbMatch.MatchId.Equals(matchId))
						{
							found = true;
							if (dbMatch.CompletionLevel == 3) break;
							dbMatch.Ended = true;
							CombineApiRecentMatchToModel(dbMatch, apiMatch);
							DataList dl = await _basicDataServices.StatisticalData(matchId);
							if (dl is null) break;
							MapDataListToRecent(dbMatch, dl);
							break;
						}
					}
					if (!found)
					{
						RecentMatchModel rm = new RecentMatchModel();
						rm.Ended = true;
						CombineApiRecentMatchToModel(rm, apiMatch);
						DataList dl = await _basicDataServices.StatisticalData(matchId);
						if (dl is null) break;
						MapDataListToRecent(rm, dl);
						matchesToAdd.Add(rm);
					}
				}

				bool added = await _recentMatchRepo.AddMatches(matchesToAdd);
				bool updated = await _recentMatchRepo.UpdateIncompleteMatches(dbMatches);

				return added && updated;

			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}


		public async Task CompleteMatchInfo(RecentMatchModel match)
		{
			try
			{
				List<ApiRecentMatch> matches = await _basicDataServices.MatchRecent(match.MatchId);
				if(matches is null || matches.Count == 0) return;
				ApiRecentMatch apiMatch = matches[0];
				CombineApiRecentMatchToModel(match, apiMatch);
				DataList dl = await _basicDataServices.StatisticalData(match.MatchId);
				if (dl is null) return;
				MapDataListToRecent(match, dl);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return;
			}
		}



		public async Task<bool> SeedMatchesInMemory()
		{
			try
			{
				List<RecentMatchModel> matches = await _recentMatchRepo.GetThirtyDaysCompletedMatches();
				matches = matches.Where(m => m.HomeTeamId is not null && m.AwayTeamId is not null && m.Ended).OrderByDescending(m => m.MatchStartTimeOfficial).ToList();
				//var test = matches.Where(m => m.HomeTeamId.Equals("23xmvkh31ldqg8n") || m.AwayTeamId.Equals("23xmvkh31ldqg8n")).ToList();
				//Dictionary<string, List<RecentMatchModel>> hashMatches = _inMemory.GetPastMatchesHash();
				foreach (var match in matches) {
					_inMemory.AddPastMatchHash(match);
                }
				//_inMemory.SetRecentMatchModel(matches);
				//_inMemory.SetPastMatchesHash(hashMatches);
				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}


		private void CombineApiRecentMatchToModel(RecentMatchModel dbMatch, ApiRecentMatch apiMatch)
		{
			int homeScore = apiMatch.HomeScores[0] + (apiMatch.HomeScores[5] == 0 ? 0 : apiMatch.HomeScores[5] - apiMatch.HomeScores[0]) + apiMatch.HomeScores[6];
			int awayScore = apiMatch.AwayScores[0] + (apiMatch.AwayScores[5] == 0 ? 0 : apiMatch.AwayScores[5] - apiMatch.AwayScores[0]) + apiMatch.AwayScores[6];

			dbMatch.MatchId = apiMatch.Id;
			dbMatch.HomeTeamId = apiMatch.HomeTeamId;
			dbMatch.AwayTeamId = apiMatch.AwayTeamId;
			dbMatch.CompetitionId = apiMatch.CompetitionId;
			dbMatch.SeasonId = apiMatch.SeasonId;
			dbMatch.HomeWin = homeScore > awayScore;
			dbMatch.HomeGoals = homeScore - apiMatch.HomeScores[6];
			dbMatch.AwayGoals = awayScore - apiMatch.AwayScores[6];
			dbMatch.GameDrawn = homeScore == awayScore;
			dbMatch.HomeCorners = apiMatch.HomeScores[4];
			dbMatch.AwayCorners = apiMatch.AwayScores[4];
			dbMatch.HomeRedCards = apiMatch.HomeScores[2];
			dbMatch.HomeYellowCards = apiMatch.HomeScores[3];
			dbMatch.AwayRedCards = apiMatch.HomeScores[2];
			dbMatch.AwayYellowCards = apiMatch.HomeScores[3];
			dbMatch.Ended = ApiStatus.badStatus.Contains(apiMatch.StatusId);
			dbMatch.AbruptEnd = apiMatch.StatusId != 8;
			dbMatch.HomeTeamHalfTimeScore = apiMatch.HomeScores[1];
			dbMatch.AwayTeamHalfTimeScore = apiMatch.AwayScores[1];
			dbMatch.MatchStartTimeOfficial = DateTimeOffset.FromUnixTimeSeconds(apiMatch.MatchTime).UtcDateTime;
			dbMatch.PenaltyShootout = apiMatch.HomeScores[6] + apiMatch.AwayScores[6] != 0;
			dbMatch.OverTime = apiMatch.HomeScores[5] + apiMatch.AwayScores[5] != 0;
			dbMatch.StageId = apiMatch.Round.StageId;
		}

		private void MapDataListToRecent(RecentMatchModel rm, DataList dl)
		{
			try
			{
				rm.MatchId = dl.MatchId;
				rm.HomeGoals = dl.HomeScore[0] + (dl.HomeScore[5] > 0 ? dl.HomeScore[5] - dl.HomeScore[0] : 0) + dl.HomeScore[6];
				rm.AwayGoals = dl.AwayScore[0] + (dl.AwayScore[5] > 0 ? dl.AwayScore[5] - dl.AwayScore[0] : 0) + dl.AwayScore[6];
				rm.HomeWin = rm.HomeGoals > rm.AwayGoals;
				rm.GameDrawn = rm.HomeGoals == rm.AwayGoals;
				rm.HomeCorners = dl.HomeScore[4];
				rm.AwayCorners = dl.AwayScore[4];
				rm.HomeRedCards = dl.HomeScore[2];
				rm.AwayRedCards = dl.AwayScore[2];
				rm.HomeYellowCards = dl.HomeScore[3];
				rm.AwayYellowCards = dl.HomeScore[3];

				rm.HomeTeamHalfTimeScore = dl.HomeScore[1];
				rm.AwayTeamHalfTimeScore = dl.AwayScore[1];

				foreach (Stat st in dl.Stats)
				{
					int type = st.Type;
					int home = st.Home;
					int away = st.Away;
					switch (type)
					{
						case 8:
							rm.HomePenalties = home == 0 ? rm.HomePenalties : home;
							rm.AwayPenalties = away == 0 ? rm.AwayPenalties : away;
							break;
						case 17:
							rm.HomeOwnGoals = home == 0 ? rm.HomeOwnGoals : home;
							rm.AwayOwnGoals = away == 0 ? rm.AwayOwnGoals : away;
							break;
						case 21:
							rm.HomeShotsOnTarget = home == 0 ? rm.HomeShotsOnTarget : home;
							rm.AwayShotsOnTarget = away == 0 ? rm.AwayShotsOnTarget : away;
							break;
						case 22:
							rm.HomeShotsOffTarget = home == 0 ? rm.HomeShotsOffTarget : home;
							rm.AwayShotsOffTarget = away == 0 ? rm.AwayShotsOffTarget : away;
							break;
						case 23:
							rm.HomeAttacks = home == 0 ? rm.HomeAttacks : home;
							rm.AwayAttacks = away == 0 ? rm.AwayAttacks : away;
							break;
						case 24:
							rm.HomeDangerousAttacks = home == 0 ? rm.HomeDangerousAttacks : home;
							rm.AwayDangerousAttacks = away == 0 ? rm.AwayDangerousAttacks : away;
							break;
						case 25:
							rm.HomePossession = home == 0 ? rm.HomePossession : home;
							rm.AwayPossession = away == 0 ? rm.AwayPossession : away;
							break;
					}
				}

				StringBuilder homeSub = new StringBuilder();
				StringBuilder awaySub = new StringBuilder();
				StringBuilder homeGoals = new();
				StringBuilder awayGoals = new();
				StringBuilder homeYellow = new();
				StringBuilder awayYellow = new();
				StringBuilder homeRed = new();
				StringBuilder awayRed = new();
				StringBuilder homePenalties = new();
				StringBuilder awayPenalties = new();
				rm.HomeScorers = "";
				rm.AwayScorers = "";
				foreach (Incident inc in dl.Incidents)
				{
					int type = inc.Type;
					int position = inc.Position;
					int time = inc.Time;
					if (type == 9)
					{
						if (position == 1)
							homeSub.Append(time + ",");
						else
							awaySub.Append(time + ",");
					}
					else if (type == 1 || type == 8 || type == 17)
					{
						string player = string.IsNullOrEmpty(inc.PlayerName) ? "empt" : inc.PlayerName;
						if (position == 1)
						{
							homeGoals.Append(time + ",");
							rm.HomeScorers += player + ",";
						}
						else
						{
							awayGoals.Append(time + ",");
							rm.AwayScorers += player + ",";
						}
					}
					else if (type == 3)
					{
						if (position == 1)
							homeYellow.Append(time + ",");
						else
							awayYellow.Append(time + ",");
					}
					else if (type == 4 || type == 15)
					{
						if (position == 1)
							homeRed.Append(time + ",");
						else
							awayRed.Append(time + ",");
					}
					if (type == 8)
					{
						if (position == 1)
							homePenalties.Append("g");
						else
							awayPenalties.Append("g");
					}
					else if (type == 16)
					{
						if (position == 1)
							homePenalties.Append("x");
						else
							awayPenalties.Append("g");
					}
				}

				

				RemoveComma(homeSub);
				RemoveComma(awaySub);
				RemoveComma(homeGoals);
				RemoveComma(awayGoals);
				RemoveComma(homeYellow);
				RemoveComma(awayYellow);
				RemoveComma(homeRed);
				RemoveComma(awayRed);

				rm.HomeScorers = rm.HomeScorers.TrimEnd(',');
				rm.AwayScorers = rm.AwayScorers.TrimEnd(',');

				rm.HomeGoalMinutes = string.IsNullOrEmpty(homeGoals.ToString()) ? rm.HomeGoalMinutes : homeGoals.ToString();
				rm.AwayGoalMinutes = string.IsNullOrEmpty(awayGoals.ToString()) ? rm.AwayGoalMinutes : awayGoals.ToString();
				rm.HomeSubstitutionMinutes = string.IsNullOrEmpty(homeSub.ToString()) ? rm.HomeSubstitutionMinutes : homeSub.ToString();
				rm.AwaySubstitutionMinutes = string.IsNullOrEmpty(awaySub.ToString()) ? rm.AwaySubstitutionMinutes : awaySub.ToString();
				rm.HomeRedMinutes = string.IsNullOrEmpty(homeRed.ToString()) ? rm.HomeRedMinutes : homeRed.ToString();
				rm.AwayRedMinutes = string.IsNullOrEmpty(awayRed.ToString()) ? rm.AwayRedMinutes : awayRed.ToString();
				rm.HomeYellowMinutes = string.IsNullOrEmpty(homeYellow.ToString()) ? rm.HomeYellowMinutes : homeYellow.ToString();
				rm.AwayYellowMinutes = string.IsNullOrEmpty(awayGoals.ToString()) ? rm.AwayYellowMinutes : awayYellow.ToString();

				rm.HomePenaltiesRecord = homePenalties.ToString();
				rm.AwayPenaltiesRecord = awayPenalties.ToString();

				rm.Ended = dl.Status == 8;
				rm.AbruptEnd = dl.Status != 8;

				rm.MatchStartTimeOfficial = rm.MatchStartTimeOfficial;
				rm.OverTime = rm.OverTime;
				rm.PenaltyShootout = rm.PenaltyShootout;


				rm.Ended = true;
				rm.CompleteInfo = false;
				rm.CompletionLevel = 3;

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return;
			}
		}

		private void RemoveComma(StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.Remove(sb.Length - 1, 1);
			}
		}

		private bool StringExistsInList(List<string> li, string target)
		{
			try
			{
				if (li is null) return true;
				int low = 0, high = li.Count - 1;
				while (low <= high)
				{
					int mid = (low + high) / 2;
					string midValue = li[mid];
					int compareResult = string.Compare(midValue, target);
					if (compareResult == 0)
						return true;
					else if (compareResult < 0)
						low = mid + 1;
					else
						high = mid - 1;
				}
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return true;
			}
		}
	}
}
