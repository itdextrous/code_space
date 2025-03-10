using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.Entities
{
    public class Opportunities
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string MatchId { get; set; }


        public int UnderZeroPointFiveGoals { get; set; }
        public int OverZeroPointFiveGoals { get; set; }

        public int UnderOnePointFiveGoals { get; set; }
        public int OverOnePointFiveGoals { get; set; }

        public int UnderTwoPointFiveGoals { get; set; }
        public int OverTwoPointFiveGoals { get; set; }

        public int UnderThreePointFiveGoals { get; set; }
        public int OverThreePointFiveGoals { get; set; }

        public int UnderFourPointFiveGoals { get; set; }
        public int OverFourPointFiveGoals { get; set; }

        public int UnderFivePointFiveGoals { get; set; }
        public int OverFivePointFiveGoals { get; set; }
        public int UnderSixPointFiveGoals { get; set; }
        public int OverSixPointFiveGoals { get; set; }

        public int UnderSevenPointFiveGoals { get; set; }
        public int OverSevenPointFiveGoals { get; set; }

        public int UnderEightPointFiveGoals { get; set; }
        public int OverEightPointFiveGoals { get; set; }

        public int UnderNinePointFiveGoals { get; set; }
        public int OverNinePointFiveGoals { get; set; }

        public int UnderZeroPointFiveCorners { get; set; }
        public int OverZeroPointFiveCorners { get; set; }

        public int UnderOnePointFiveCorners { get; set; }
        public int OverOnePointFiveCorners { get; set; }

        public int UnderTwoPointFiveCorners { get; set; }
        public int OverTwoPointFiveCorners { get; set; }

        public int UnderThreePointFiveCorners { get; set; }
        public int OverThreePointFiveCorners { get; set; }

        public int UnderFourPointFiveCorners { get; set; }
        public int OverFourPointFiveCorners { get; set; }

        public int UnderFivePointFiveCorners { get; set; }
        public int OverFivePointFiveCorners { get; set; }
        public int UnderSixPointFiveCorners { get; set; }
        public int OverSixPointFiveCorners { get; set; }
        public int UnderSevenPointFiveCorners { get; set; }
        public int OverSevenPointFiveCorners { get; set; }
        public int UnderEightPointFiveCorners { get; set; }
        public int OverEightPointFiveCorners { get; set; }
        public int UnderNinePointFiveCorners { get; set; }
        public int OverNinePointFiveCorners { get; set; }
        public int BTTS { get; set; }
        public int HomeTeamWin { get; set; }
        public int AwayTeamWin { get; set; }

        public int HomeTeamWinFirstHalfWin { get; set; }
        public int AwayTeamFirstHalfWin { get; set; }

        public int HomeTeamSecondHalfWin { get; set; }
        public int AwayTeamSecondHalfWin { get; set; }




        public LiveMatchModel Match { get; set; }

    }
}
