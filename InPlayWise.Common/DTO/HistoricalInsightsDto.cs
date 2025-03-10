namespace InPlayWise.Common.DTO
{
	public class HistoricalInsightsDto
	{
		public float GoalsScoredAvg { get; set; }
		public float GoalsScoredFirstHalfAvg { get; set; }
		public float GoalsScoredSecondHalfAvg { get; set; }
		public float GoalsConcededAvg { get; set; }
		public float GoalsConcededFirstHalfAvg { get; set; }
		public float GoalsConcededSecondHalfAvg { get; set; }
		public int ScoredFirstHalfAndSecondHalfPercent { get; set; }
		public int GoalsConcededFirstHalfAndSecondHalfPercent { get; set; }
		public float GoalsAvg { get; set; }
		public float GoalsFirstHalfAvg { get; set; }
		public float GoalsSecondHalfAvg { get; set; }
		public int GoalsFirstHalfAndSecondHalfPercent { get; set; }
		public int OverZeroPointFivePercent { get; set; }
		public int OverOnePointFivePercent { get; set; }
		public int OverTwoPointFivePercent { get; set; }
		public int OverThreePointFivePercent { get; set; }
		public int BothTeamsScoredPercent { get; set; }
		public int NoGoalScoredPercent { get; set; }
		public int HomeWinPercent { get; set; }
		public int AwayWinPercent { get; set; }
		public int ShotsOnTarget { get; set; }
		public int DangerousAttack { get; set; }
		public float ShotsOnTargetAverage { get; set; }
		public float DangerousAttacksAverage { get; set; }
		public float AverageCornersOfTeam { get; set; }
		public float AverageCornersInGame { get; set; }
		public int CleanSheetPercent { get; set; }
	}
}
