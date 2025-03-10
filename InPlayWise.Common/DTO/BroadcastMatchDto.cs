namespace InPlayWise.Common.DTO
{
	public class BroadcastMatchDto
	{
		public string MatchId { get; set; }
		public int MatchStatus { get; set; }
		public int MatchMinutes { get; set; }
		public int HomeGoals { get; set; }
		public int AwayGoals { get; set; }
		public int HomeRed { get; set; }
		public int AwayRed { get; set; }
		public int HomeYellow { get; set; }
		public int AwayYellow { get; set; }
		public int HomeCorners { get; set; }
		public int AwayCorners { get; set; }
		public int HomeShotsOnTarget { get; set; }
		public int HomeShotsOffTarget { get; set; }
		public int AwayShotsOnTarget { get; set; }
		public int AwayShotsOffTarget { get; set; }
		public int HomeDangerousAttacks { get; set; }
		public int AwayDangerousAttacks { get; set; }
		public int HomeAttacks { get; set; }
		public int AwayAttacks { get; set; }
		public int HomePenalties { get; set; }
		public int AwayPenalties { get; set; }
		public int HomePossession { get; set; }
		public int AwayPossession { get; set; }
		public int HomeOwnGoals { get; set; }
		public int AwayOwnGoals { get; set; }
		public string HomeGoalMinutes { get; set; } = "";
		public string AwayGoalMinutes { get; set; } = "";
		public string HomeSubstitutionMinutes { get; set; } = "";
		public string HomeSubstitutionNames { get; set; } = "";
		public string AwaySubstitutionMinutes { get; set; } = "";
		public string AwaySubstitutionNames { get; set; } = "";
		public string HomeRedMinutes { get; set; } = "";
		public string HomeRedNames { get; set; } = "";
		public string AwayRedMinutes { get; set; } = "";
		public string AwayRedNames { get; set; } = "";
		public string HomeYellowMinutes { get; set; } = "";
		public string HomeYellowNames { get; set; } = "";
		public string AwayYellowMinutes { get; set; } = "";
		public string AwayYellowNames { get; set; } = "";
		public string HomeCornerMinutes { get; set; } = "";
		public string AwayCornerMinutes { get; set; } = "";
		public string HomeShotsOnTargetMinutes { get; set; } = "";
		public string AwayShotsOnTargetMinutes { get; set; } = "";
		public string HomeShotsMinutes { get; set; } = "";
		public string AwayShotsMinutes { get; set; } = "";
		public string HomeScorers { get; set; } = "";
		public string AwayScorers { get; set; } = "";
		public string HomePenaltiesRecord { get; set; } = "";
		public string AwayPenaltiesRecord { get; set; } = "";
		public bool OverTime { get; set; }
		public bool PenaltyShootOut { get; set; }
		public int HomeTeamHalfTimeScore { get; set; }
		public int HomeTeamOverTimeScore { get; set; }
		public int HomeTeamPenaltyShootOutScore { get; set; }
		public int AwayTeamHalfTimeScore { get; set; }
		public int AwayTeamOverTimeScore { get; set; }
		public int AwayTeamPenaltyShootoutScore { get; set; }
		public int NumOfOpportunities { get; set; }

	}
}
