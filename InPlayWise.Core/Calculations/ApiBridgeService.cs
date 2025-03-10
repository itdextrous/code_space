using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Data.Entities.SportsEntities;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Calculations
{
    public class ApiBridgeService
	{
		private readonly ILogger<ApiBridgeService> _logger;
		public ApiBridgeService(ILogger<ApiBridgeService> logger) {
			_logger = logger;
		}

		public RecentMatchModel ApiRecentMatchToModel(ApiRecentMatch apiMatch)
		{
			try
			{
				RecentMatchModel m = new RecentMatchModel();
				int homeScore = apiMatch.HomeScores[0] + (apiMatch.HomeScores[5] == 0 ? 0 : apiMatch.HomeScores[5] - apiMatch.HomeScores[0]) + apiMatch.HomeScores[6];
				int awayScore = apiMatch.AwayScores[0] + (apiMatch.AwayScores[5] == 0 ? 0 : apiMatch.AwayScores[5] - apiMatch.AwayScores[0]) + apiMatch.AwayScores[6];
				m.MatchId = apiMatch.Id;
				m.HomeTeamId = apiMatch.HomeTeamId;
				m.AwayTeamId = apiMatch.AwayTeamId;
				m.CompetitionId = apiMatch.CompetitionId;
				m.SeasonId = apiMatch.SeasonId;
				m.HomeWin = homeScore > awayScore;
				m.HomeGoals = homeScore;
				m.AwayGoals = awayScore;
				m.GameDrawn = homeScore == awayScore;
				m.HomeCorners = apiMatch.HomeScores[4];
				m.AwayCorners = apiMatch.AwayScores[4];
				m.HomeRedCards = apiMatch.HomeScores[2];
				m.HomeYellowCards = apiMatch.HomeScores[3];
				m.AwayRedCards = apiMatch.HomeScores[2];
				m.AwayYellowCards = apiMatch.HomeScores[3];
				m.Ended = apiMatch.StatusId == 8;
				m.AbruptEnd = apiMatch.StatusId != 8;
				m.HomeTeamHalfTimeScore = apiMatch.HomeScores[1];
				m.AwayTeamHalfTimeScore = apiMatch.AwayScores[1];
				m.MatchStartTimeOfficial = DateTimeOffset.FromUnixTimeSeconds(apiMatch.MatchTime).UtcDateTime;
				return m;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}




	}
}
