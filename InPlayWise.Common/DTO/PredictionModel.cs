using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Common.DTO
{
    public class PredictionModel
    {
        [Key]
        public Guid Id { get; set; }
        public string MatchId { get; set; }
        public short MatchStatus { get; set; }
        public short MatchMinutes { get; set; }
        public short ExtraTime { get; set; }
        public short HomeGoals { get; set; }
        public short AwayGoals { get; set; }
        public short HomeRed { get; set; }
        public short AwayRed { get; set; }
        public short HomeYellow { get; set; }
        public short AwayYellow { get; set; }
        public short HomeCorners { get; set; }
        public short AwayCorners { get; set; }
        public short HomeShotsOnTarget { get; set; }
        public short HomeShotsOffTarget { get; set; }
        public short AwayShotsOnTarget { get; set; }
        public short AwayShotsOffTarget { get; set; }
        public short Incident { get; set; }
        public short HomeDangerousAttacks { get; set; }
        public short AwayDangerousAttacks { get; set; }
        public short HomeAttacks { get; set; }
        public short AwayAttacks { get; set; }
        public short HomePenalties { get; set; }
        public short AwayPenalties { get; set; }
        public short HomePossession { get; set; }
        public short AwayPossession { get; set; }
        public short HomeOwnGoals { get; set; }
        public short AwayOwnGoals { get; set; }

    }
}
