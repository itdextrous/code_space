using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.DTO.OpportunitiesEntities
{
	public class OpportunitiesRequestDto
	{
		public List<MatchCompleteData> AllMatchesData { get; set; }

		public OpportunitiesRequestDto() { }

		public OpportunitiesRequestDto(List<LiveMatchModel> matches)
		{
			List<MatchCompleteData> allMatches = new List<MatchCompleteData>();

			foreach(LiveMatchModel match in matches)
			{
				MatchCompleteData md = new MatchCompleteData();
				md.MatchInfo = new MatchInfo()
				{
					MatchId = match.MatchId,
					CompetitionId = match.CompetitionId,
					CompetitionName = match.Competition is not null ? match.Competition.Name : "",
					HomeTeamName = match.HomeTeam is null ? "" : match.HomeTeam.Name ,
					HomeTeamId = match.HomeTeamId,
					AwayTeamName = match.AwayTeam is null ? "" : match.AwayTeam.Name,
					AwayTeamId = match.AwayTeamId,
					HomeGoals = (byte)match.HomeGoals,
					AwayGoals = (byte)match.AwayGoals,
					HomeCorners = (byte)match.HomeCorners,
					AwayCorners = (byte)match.AwayCorners,
					HomeTeamLogo = match.HomeTeam is null ? "" : match.HomeTeam.Logo,
					AwayTeamLogo = match.AwayTeam is null ? "" : match.AwayTeam.Logo,
					CompetitionLogo = match.Competition is null ? "" : match.Competition.Logo
				};
				md.MatchData = new MatchData()
				{
					MatchMinutes = (byte)match.MatchMinutes ,
					HomeGoals = (byte)match.HomeGoals ,
					AwayGoals = (byte)match.AwayGoals ,
					HomeRed = (byte)match.HomeRed ,
					AwayRed = (byte)match.AwayRed ,
					HomeYellow = (byte)match.HomeYellow ,
					AwayYellow = (byte)match.AwayYellow ,
					HomeCorners = (byte)match.HomeCorners ,
					AwayCorners = (byte)match.AwayCorners ,
					HomeShotsOnTarget = (byte)match.HomeShotsOnTarget ,
					HomeShotsOffTarget = (byte)match.HomeShotsOffTarget ,
					AwayShotsOnTarget = (byte)match.AwayShotsOnTarget ,
					AwayShotsOffTarget = (byte)match.AwayShotsOffTarget ,
					HomeDangerousAttacks = (byte)match.HomeDangerousAttacks ,
					AwayDangerousAttacks = (byte)match.AwayDangerousAttacks ,
					HomeAttacks = (byte)match.HomeAttacks ,
					AwayAttacks = (byte)match.AwayAttacks ,
					HomePenalties = (byte)match.HomePenalties ,
					AwayPenalties = (byte)match.AwayPenalties ,
					HomePossession = (byte)match.HomePossession ,
					AwayPossession = (byte)match.AwayPossession ,
				};
				allMatches.Add(md);
			}

			AllMatchesData = allMatches;
		}

		public class MatchCompleteData
		{
			public MatchInfo MatchInfo { get; set; }
			public MatchData MatchData { get; set; }
		}

		public class MatchInfo
		{
			public string MatchId { get; set; }
			public string CompetitionName { get; set; }
			public string CompetitionId { get; set; }
			public string HomeTeamName { get; set; }
			public string HomeTeamId { get; set; }
			public string AwayTeamName { get; set; }
			public string AwayTeamId { get; set; }
			public byte HomeCorners { get; set;}
			public byte AwayCorners { get; set; }
			public byte HomeGoals { get; set; }
			public byte AwayGoals { get; set; }
			public string HomeTeamLogo { get; set; }
			public string AwayTeamLogo { get; set; }
			public string CompetitionLogo { get; set; }

		}

		public class MatchData
		{
			public byte MatchMinutes { get; set; }
			public byte HomeGoals { get; set; }
			public byte AwayGoals { get; set; }
			public byte HomeRed { get; set; }
			public byte AwayRed { get; set; }
			public byte HomeYellow { get; set; }
			public byte AwayYellow { get; set; }
			public byte HomeCorners { get; set; }
			public byte AwayCorners { get; set; }
			public byte HomeShotsOnTarget { get; set; }
			public byte HomeShotsOffTarget { get; set; }
			public byte AwayShotsOnTarget { get; set; }
			public byte AwayShotsOffTarget { get; set; }
			public byte HomeDangerousAttacks { get; set; }
			public byte AwayDangerousAttacks { get; set; }
			public byte HomeAttacks { get; set; }
			public byte AwayAttacks { get; set; }
			public byte HomePenalties { get; set; }
			public byte AwayPenalties { get; set; }
			public byte HomePossession { get; set; }
			public byte AwayPossession { get; set; }
		}


	}
}
