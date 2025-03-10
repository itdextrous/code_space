namespace InPlayWise.Data.DTO.PredictionDto
{
    public class Features
    {
        public string matchId { get; set; }
        public short matchStatus { get; set; }
        public short matchMinutes { get; set; }
        public short extraTime { get; set; }
        public short homeGoals { get; set; }
        public short awayGoals { get; set; }
        public short homeRed { get; set; }
        public short awayRed { get; set; }
        public short homeYellow { get; set; }
        public short awayYellow { get; set; }
        public short homeCorners { get; set; }
        public short awayCorners { get; set; }
        public short homeShotsOnTarget { get; set; }
        public short homeShotsOffTarget { get; set; }
        public short awayShotsOnTarget { get; set; }
        public short awayShotsOffTarget { get; set; }
        public short incident { get; set; }
        public short homeDangerousAttacks { get; set; }
        public short awayDangerousAttacks { get; set; }
        public short homeAttacks { get; set; }
        public short awayAttacks { get; set; }
        public short homePenalties { get; set; }
        public short awayPenalties { get; set; }
        public short homePossession { get; set; }
        public short awayPossession { get; set; }
        public short homeOwnGoals { get; set; }
        public short awayOwnGoals { get; set; }
    }
}
