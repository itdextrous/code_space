using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.Entities.Prediction
{
    public class PredictionActiveMatchesData
    {
        public Guid Id { get; set; }
        public string MatchId { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public string CompetitionId { get; set; }
        public DateTime MatchTime { get; set; }
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
        public int HomeTeamRank { get; set; }
        public int AwayTeamRank { get; set; }
        public int HomeTeamHalfTimeScore { get; set; }
        public int AwayTeamHalfTimeScore { get; set; }
        public int HomeTeamFullTimeScore { get; set; }
        public int AwayTeamFullTimeScore { get; set; }
        public int HomeTeamFullTimeCorners { get; set; }
        public int AwayTeamFullTimeCorners { get; set; }

        public LiveMatchModel match { get; set; }

    }
}
