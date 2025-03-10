using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Common.DTO
{
    public class RecentMatchDto
    {
        [Key]
        public string MatchId { get; set; }
        public string HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamLogo { get; set; }
        public string CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionLogo { get; set; }
        public string SeasonId { get; set; }
        public bool HomeWin { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public bool GameDrawn { get; set; }
        public int HomeCorners { get; set; }
        public int AwayCorners { get; set; }
        public int HomeRedCards { get; set; }
        public int HomeYellowCards { get; set; }
        public int AwayRedCards { get; set; }
        public int AwayYellowCards { get; set; }
        public string HomeGoalMinutes { get; set; } = "";
        public string AwayGoalMinutes { get; set; } = "";
        public int HomeShotsOnTarget { get; set; }
        public int AwayShotsOnTarget { get; set; }
        public int HomeShotsOffTarget { get; set; }
        public int AwayShotsOffTarget { get; set; }
        public int HomeAttacks { get; set; }
        public int AwayAttacks { get; set; }
        public int HomeDangerousAttacks { get; set; }
        public int AwayDangerousAttacks { get; set; }
        public int HomePossession { get; set; }
        public int AwayPossession { get; set; }
        public int HomePenalties { get; set; }
        public int AwayPenalties { get; set; }
        public int HomeOwnGoals { get; set; }
        public int AwayOwnGoals { get; set; }
        public string HomeSubstitutionMinutes { get; set; } = "";
        public string AwaySubstitutionMinutes { get; set; } = "";
        public string HomeRedMinutes { get; set; } = "";
        public string AwayRedMinutes { get; set; } = "";
        public string HomeYellowMinutes { get; set; } = "";
        public string AwayYellowMinutes { get; set; } = "";
        public string HomeCornerMinutes { get; set; } = "";
        public string AwayCornerMinutes { get; set; } = "";
        public string HomeShotsOnTargetMinutes { get; set; } = "";
        public string AwayShotsOnTargetMinutes { get; set; } = "";
        public string HomeShotsMinutes { get; set; } = "";
        public string AwayShotsMinutes { get; set; } = "";
        public string HomeScorers { get; set; } = "";
        public string AwayScorers { get; set; } = "";
        public bool Ended { get; set; }
        public bool AbruptEnd { get; set; }
        public int HomeTeamHalfTimeScore { get; set; }
        public int AwayTeamHalfTimeScore { get; set; }

        [NotMapped]
        public bool HomeTeamComebackToWin { get; set; }
        [NotMapped]
        public bool AwayTeamComebackToWin { get; set; }
        [NotMapped]
        public bool HomeTeamComebackToDraw { get; set; }
        [NotMapped]
        public bool AwayTeamComebackToDraw { get; set; }
        [NotMapped]
        public bool HomeScoredFirstAndWin { get; set; }
        [NotMapped]
        public bool AwayScoredFirstAndWin { get; set; }
        public DateTime MatchStartTimeOfficial { get; set; }
        //public long MatchStartTime { get; set; }
        public int TotalMatchMinutes { get; set; }
        public bool OverTime { get; set; }
        public bool PenaltyShootout { get; set; }
        public bool CompleteInfo { get; set; }
        public int CompletionLevel { get; set; }
        public string HomePenaltiesRecord { get; set; } = "";
        public string AwayPenaltiesRecord { get; set; } = "";
        public string HomeSubstituteNames { get; set; } = "";
        public string AwaySubstituteNames { get; set; } = "";
        public string HomeYellowNames { get; set; } = "";
        public string AwayYellowNames { get; set; } = "";
        public string HomeRedNames { get; set; } = "";
        public string AwayRedNames { get; set; } = "";
        public bool StatsComplete { get; set; } = false;

        [NotMapped]
        public int HomeTeamPosition { get; set; }
        [NotMapped]
        public int AwayTeamPosition { get; set; }
        [NotMapped]
        public int RoundNum { get; set; }
        [NotMapped]
        public int GroupNum { get; set; }
        [NotMapped]
        public string StageId { get; set; }
    }
}
