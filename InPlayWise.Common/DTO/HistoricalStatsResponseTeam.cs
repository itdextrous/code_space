namespace InPlayWise.Common.DTO
{
    public class HistoricalStatsResponseTeam
    {

        public string TeamId { get; set;}
        public float AvgPossession { get; set; }
        public List<string> MatchResult { get; set; } = new List<string>();
        public List<bool> ScoredFirstAndWon { get; set; } = new List<bool>();
        public float AvgShots { get; set; }
        public float AvgShotsOnTargetPercentage { get; set; }
        public float AvgAttacks { get; set; }
        public float AvgDangerousAttacksPercentage { get; set; }
        public float AvgRedCards { get; set; }
        public float AvgYellowCards { get; set; }

        public float AvgShotsOnTarget { get; set; }
        public float AvgShotConcededMinute { get; set; }
        public List<bool> Penalties { get; set; } = new List<bool>(3);

        // goals selected
        public int TotalGoals { get; set; }
        public float AvgGoalsScored { get; set; }
        public float AvgGoalsTime { get; set; }
        public List<float> AvgGoalsScoredInterval { get; set; } = new List<float>(3);


        // for corners selected
        public float AvgCorners { get; set; }
        public float AvgCornersTime { get; set; }
        public List<float> AvgCornersInterval { get; set; } = new List<float>(3);


        public List<string> CornerMinutes { get; set; } = new List<string>();
        public List<string> GoalMinutes { get; set; } = new List<string>();
        public List<string> ShotsOnTargetMinutes { get; set; } = new List<string>();




        public List<RecentMatchDto> pastMatches { get; set; } = new List<RecentMatchDto>();

    }
}
