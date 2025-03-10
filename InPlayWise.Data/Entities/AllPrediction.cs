namespace InPlayWise.Data.Entities
{
    public class AllPrediction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int HomeGoals { get; set; }
        public int HomeShotsOnTarget { get; set; }
        public int HomeShotsOffTarget { get; set; }
        public int HomeAttacks { get; set; }
        public int HomeDangerousAttacks { get; set; }
        public int HomeCorners { get; set; }
        public int HomePenalties { get; set; }
        //public double HomeAvgTimeToGoal { get; set; }
        // Add other home-related fields as needed

        // Away Team
        public int AwayGoals { get; set; }
        public int AwayShotsOnTarget { get; set; }
        public int AwayShotsOffTarget { get; set; }
        public int AwayAttacks { get; set; }
        public int AwayDangerousAttacks { get; set; }
        public int AwayCorners { get; set; }
        public int AwayPenalties { get; set; }
        //double AwayAvgTimeToGoal { get; set; }
        // Add other away-related fields as needed

        // General Match Information
        public int MatchMinutes { get; set; }
        public int TotalScore { get; set; }
        public int TimeFromLastGoal { get; set; }
        public int TimeFromLastGoalHome { get; set; }
        public int TimeFromLastGoalAway { get; set; }

        public int CPGoalFirst10Minutes { get; set; }
        //public bool IsSecondHalf { get; set; }

        // Ratios and Averages
        public double TotalShotsOnOffRatio { get; set; }
        public double HomeShotsOnOffRatio { get; set; }
        public double AwayShotsOnOffRatio { get; set; }
        public double TotalAttacksRatio { get; set; }
        public double HomeAttacksRatio { get; set; }
        public double AwayAttacksRatio { get; set; }
        public double TotalCornersRatio { get; set; }
        public double HomeCornersRatio { get; set; }
        public double AwayCornersRatio { get; set; }
        public double TotalPenaltiesRatio { get; set; }
        public double HomePenaltiesRatio { get; set; }
        public double AwayPenaltiesRatio { get; set; }



        public float HomeAvgTimeToGoal { get; set; }
        public float AwayAvgTimeToGoal { get; set; }
        public float AvgTimeToGoal { get; set; }




        // Identity Fields
        public int Identity00 { get; set; }
        public int Identity01 { get; set; }
        public int Identity02 { get; set; }
        public int Identity03 { get; set; }
        public int Identity04 { get; set; }
        public int Identity05 { get; set; }
        public int Identity06 { get; set; }
        public int Identity07 { get; set; }
        public int Identity08 { get; set; }

        public int Identity11 { get; set; }
        public int Identity12 { get; set; }
        public int Identity13 { get; set; }
        public int Identity14 { get; set; }
        public int Identity15 { get; set; }
        public int Identity16 { get; set; }
        public int Identity17 { get; set; }
        public int Identity18 { get; set; }

        public int Identity0Over8 { get; set; }
        public int Identity1Over8 { get; set; }

        public int Identity22 { get; set; }
        public int Identity23 { get; set; }
        public int Identity24 { get; set; }
        public int Identity25 { get; set; }
        public int Identity26 { get; set; }
        public int Identity27 { get; set; }
        public int Identity28 { get; set; }

        public int Identity2Over8 { get; set; }

        public int Identity33 { get; set; }
        public int Identity34 { get; set; }
        public int Identity35 { get; set; }
        public int Identity36 { get; set; }
        public int Identity37 { get; set; }
        public int Identity38 { get; set; }

        public int Identity3Over8 { get; set; }

        public int Identity44 { get; set; }
        public int Identity45 { get; set; }
        public int Identity46 { get; set; }
        public int Identity47 { get; set; }
        public int Identity48 { get; set; }

        public int Identity4Over8 { get; set; }

        public int Identity55 { get; set; }
        public int Identity56 { get; set; }
        public int Identity57 { get; set; }
        public int Identity58 { get; set; }

        public int Identity5Over8 { get; set; }

        public int Identity66 { get; set; }
        public int Identity67 { get; set; }
        public int Identity68 { get; set; }

        public int Identity6Over8 { get; set; }

        public int Identity77 { get; set; }
        public int Identity78 { get; set; }

        public int Identity7Over8 { get; set; }

        public int Identity88 { get; set; }

        public int IdentityOver8Over8 { get; set; }
        // Add other identity fields as needed

        // ... Continue adding properties for the remaining fields

        // Ratios and Averages (Log and Power)

        public float DangerousAttacksMinuteRation { get; set; }
        public float HomeDangerousAttacksMinuteRatio { get; set; }
        public float AwayDangerousAttacksMinuteRation { get; set; }
        public float DangerousAttacksRatio { get; set; }
        public float HomeDangerousAttacksRatio { get; set; }
        public float AwayDangerouAttacksRatio { get; set; }

        public bool IsSecondHalf { get; set; }

        public int GoalDifference { get; set; }

        public int Time2 { get; set; }

        public float TimeLog { get; set; }
        public int Time3 { get; set; }

        public double TotalAttacksRatioLog { get; set; }
        public double TotalAttacksRatio2 { get; set; }
        public double HomeAttacksRatioLog { get; set; }
        public double HomeAttacksRatio2 { get; set; }
        public double AwayAttacksRatioLog { get; set; }
        public double AwayAttacksRatio2 { get; set; }

        public double TotalPenaltiesRatioLog { get; set; }
        public double TotalPenaltiesRatio2 { get; set; }
        public double HomePenaltiesRatioLog { get; set; }
        public double HomePenaltiesRatio2 { get; set; }
        public double AwayPenaltiesRatioLog { get; set; }
        public double AwayPenaltiesRatio2 { get; set; }

        public double TotalCornersRatioLog { get; set; }
        public double TotalCornersRatio2 { get; set; }
        public double HomeCornersRatioLog { get; set; }
        public double HomeCornersRatio2 { get; set; }
        public double AwayCornersRatioLog { get; set; }
        public double AwayCornersRatio2 { get; set; }

        // Dangerous Attacks Ratios and Averages (Log and Power)
        public double DangerousAttacksMinuteRatioLog { get; set; }
        public double DangerousAttacksMinuteRatio2 { get; set; }
        public double HomeDangerousAttacksMinuteRatioLog { get; set; }
        public double HomeDangerousAttacksMinuteRatio2 { get; set; }
        public double AwayDangerousAttacksMinuteRatioLog { get; set; }
        public double AwayDangerousAttacksMinuteRatio2 { get; set; }
        public double DangerousAttacksRatioLog { get; set; }
        public double DangerousAttacksRatio2 { get; set; }
        public double HomeDangerousAttacksRatioLog { get; set; }
        public double HomeDangerousAttacksRatio2 { get; set; }

        // Goal Difference

        public double GoalDiffLog { get; set; }
        public double GoalDiff2 { get; set; }
    }
}
