namespace InPlayWise.Data.Entities.PredictionEntities
{
    public class AllPredictionDataset
    {
        public Guid Id { get; set; } = Guid.NewGuid();
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



        public AllPredictionDataset() { }


        public AllPredictionDataset(DailyPredictionDataset data)
        {
			Id  = Guid.NewGuid();
            MatchMinutes = data.MatchMinutes ;
		    HomeGoals = data.HomeGoals ;
		    AwayGoals = data.AwayGoals ;
		    HomeRed = data.HomeRed ;
		    AwayRed = data.AwayRed ;
		    HomeYellow = data.HomeYellow ;
		    AwayYellow = data.AwayYellow ;
		    HomeCorners = data.HomeCorners ;
		    AwayCorners = data.AwayCorners ;
		    HomeShotsOnTarget = data.HomeShotsOnTarget ;
		    HomeShotsOffTarget = data.HomeShotsOffTarget ;
		    AwayShotsOnTarget = data.AwayShotsOnTarget ;
		    AwayShotsOffTarget = data.AwayShotsOnTarget ;
		    HomeDangerousAttacks = data.HomeDangerousAttacks ;
		    AwayDangerousAttacks = data.AwayDangerousAttacks ;
		    HomeAttacks = data.HomeAttacks ;
		    AwayAttacks = data.AwayAttacks ;
		    HomePenalties = data.HomePenalties ;
		    AwayPenalties = data.AwayPenalties ;
		    HomePossession = data.HomePossession ;
		    AwayPossession = data.AwayPossession ;
		    MatchTimeSeconds = data.MatchTimeSeconds ;
	    }


    }
}
