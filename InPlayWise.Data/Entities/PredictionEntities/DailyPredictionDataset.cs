using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.Entities.PredictionEntities
{
    public class DailyPredictionDataset
    {
        public Guid Id { get; set; } 
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
        public short MatchTimeSeconds { get; set; }
        public DailyPredictionDataset() { }
        public DailyPredictionDataset(LiveMatchModel match)
        {
            Id = Guid.NewGuid();
            MatchMinutes = (byte)match.MatchMinutes;
		    HomeGoals = (byte)match.HomeGoals  ;
		    AwayGoals = (byte)match.AwayGoals  ;
		    HomeRed = (byte)match.HomeRed  ;
		    AwayRed = (byte)match.AwayRed  ;
		    HomeYellow = (byte)match.HomeYellow  ;
		    AwayYellow = (byte)match.AwayYellow  ;
		    HomeCorners = (byte)match.HomeCorners  ;
		    AwayCorners = (byte)match.AwayCorners  ;
		    HomeShotsOnTarget = (byte)match.HomeShotsOnTarget  ;
		    HomeShotsOffTarget = (byte)match.HomeShotsOffTarget  ;
		    AwayShotsOnTarget = (byte)match.AwayShotsOnTarget  ;
		    AwayShotsOffTarget = (byte)match.AwayShotsOffTarget  ;
		    HomeDangerousAttacks = (byte)match.HomeDangerousAttacks  ;
		    AwayDangerousAttacks = (byte)match.AwayDangerousAttacks  ;
		    HomeAttacks = (byte)match.HomeAttacks  ;
		    AwayAttacks = (byte)match.AwayAttacks  ;
		    HomePenalties = (byte)match.HomePenalties  ;
		    AwayPenalties = (byte)match.AwayPenalties  ;
		    HomePossession = (byte)match.HomePossession  ;
		    AwayPossession = (byte)match.AwayPossession  ;
	    }
    }
}
