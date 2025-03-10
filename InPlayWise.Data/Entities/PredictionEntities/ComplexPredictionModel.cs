using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.Entities.PredictionEntities
{
	public class ComplexPredictionModel
    {
        public byte HomeGoals { get; set; }
        public byte HomeShotsOnTarget { get; set; }
        public byte HomeShotsOffTarget { get; set; }
        public byte HomeAttacks { get; set; }
        public byte HomeDangerousAttacks { get; set; }
        public byte HomeCorners { get; set; }
        public byte HomePenalties { get; set; }
        //public float HomeAvgTimeToGoal { get; set; }
        // Add other home-related fields as needed

        // Away Team
        public byte AwayGoals { get; set; }
        public byte AwayShotsOnTarget { get; set; }
        public byte AwayShotsOffTarget { get; set; }
        public byte AwayAttacks { get; set; }
        public byte AwayDangerousAttacks { get; set; }
        public byte AwayCorners { get; set; }
        public byte AwayPenalties { get; set; }
        //float AwayAvgTimeToGoal { get; set; }
        // Add other away-related fields as needed

        // General Match Information
        public byte MatchMinutes { get; set; }
        public byte TotalScore { get; set; }
        public byte TimeFromLastGoal { get; set; }
        public byte TimeFromLastGoalHome { get; set; }
        public byte TimeFromLastGoalAway { get; set; }

        public bool CPGoalFirst10Minutes { get; set; }
        //public bool IsSecondHalf { get; set; }

        // Ratios and Averages
        public float TotalShotsOnOffRatio { get; set; }
        public float HomeShotsOnOffRatio { get; set; }
        public float AwayShotsOnOffRatio { get; set; }
        public float TotalAttacksRatio { get; set; }
        public float HomeAttacksRatio { get; set; }
        public float AwayAttacksRatio { get; set; }
        public float TotalCornersRatio { get; set; }
        public float HomeCornersRatio { get; set; }
        public float AwayCornersRatio { get; set; }
        public float TotalPenaltiesRatio { get; set; }
        public float HomePenaltiesRatio { get; set; }
        public float AwayPenaltiesRatio { get; set; }



        public float HomeAvgTimeToGoal { get; set; }
        public float AwayAvgTimeToGoal { get; set; }
        public float AvgTimeToGoal { get; set; }

        // Identity Fields
        public bool Identity00 { get; set; }
        public bool Identity01 { get; set; }
        public bool Identity02 { get; set; }
        public bool Identity03 { get; set; }
        public bool Identity04 { get; set; }
        public bool Identity05 { get; set; }
        public bool Identity06 { get; set; }
        public bool Identity07 { get; set; }
        public bool Identity08 { get; set; }

        public bool Identity11 { get; set; }
        public bool Identity12 { get; set; }
        public bool Identity13 { get; set; }
        public bool Identity14 { get; set; }
        public bool Identity15 { get; set; }
        public bool Identity16 { get; set; }
        public bool Identity17 { get; set; }
        public bool Identity18 { get; set; }

        public bool Identity0Over8 { get; set; }
        public bool Identity1Over8 { get; set; }

        public bool Identity22 { get; set; }
        public bool Identity23 { get; set; }
        public bool Identity24 { get; set; }
        public bool Identity25 { get; set; }
        public bool Identity26 { get; set; }
        public bool Identity27 { get; set; }
        public bool Identity28 { get; set; }

        public bool Identity2Over8 { get; set; }

        public bool Identity33 { get; set; }
        public bool Identity34 { get; set; }
        public bool Identity35 { get; set; }
        public bool Identity36 { get; set; }
        public bool Identity37 { get; set; }
        public bool Identity38 { get; set; }

        public bool Identity3Over8 { get; set; }

        public bool Identity44 { get; set; }
        public bool Identity45 { get; set; }
        public bool Identity46 { get; set; }
        public bool Identity47 { get; set; }
        public bool Identity48 { get; set; }

        public bool Identity4Over8 { get; set; }

        public bool Identity55 { get; set; }
        public bool Identity56 { get; set; }
        public bool Identity57 { get; set; }
        public bool Identity58 { get; set; }

        public bool Identity5Over8 { get; set; }

        public bool Identity66 { get; set; }
        public bool Identity67 { get; set; }
        public bool Identity68 { get; set; }
        public bool Identity6Over8 { get; set; }
        public bool Identity77 { get; set; }
        public bool Identity78 { get; set; }

        public bool Identity7Over8 { get; set; }

        public bool Identity88 { get; set; }

        public bool IdentityOver8Over8 { get; set; }

        // Ratios and Averages (Log and Power)

        public float DangerousAttacksMinuteRatio { get; set; }
        public float HomeDangerousAttacksMinuteRatio { get; set; }
        public float AwayDangerousAttacksMinuteRatio { get; set; }
        public float DangerousAttacksRatio { get; set; }
        public float HomeDangerousAttacksRatio { get; set; }
        public float AwayDangerouAttacksRatio { get; set; }

        public bool IsSecondHalf { get; set; }

        public byte GoalDifference { get; set; }

        public short Time2 { get; set; }

        public float TimeLog { get; set; }
        public int Time3 { get; set; }

        public float TotalAttacksRatioLog { get; set; }
        public float TotalAttacksRatio2 { get; set; }
        public float HomeAttacksRatioLog { get; set; }
        public float HomeAttacksRatio2 { get; set; }
        public float AwayAttacksRatioLog { get; set; }
        public float AwayAttacksRatio2 { get; set; }

        public float TotalPenaltiesRatioLog { get; set; }
        public float TotalPenaltiesRatio2 { get; set; }
        public float HomePenaltiesRatioLog { get; set; }
        public float HomePenaltiesRatio2 { get; set; }
        public float AwayPenaltiesRatioLog { get; set; }
        public float AwayPenaltiesRatio2 { get; set; }

        public float TotalCornersRatioLog { get; set; }
        public float TotalCornersRatio2 { get; set; }
        public float HomeCornersRatioLog { get; set; }
        public float HomeCornersRatio2 { get; set; }
        public float AwayCornersRatioLog { get; set; }
        public float AwayCornersRatio2 { get; set; }

        // Dangerous Attacks Ratios and Averages (Log and Power)
        public float DangerousAttacksMinuteRatioLog { get; set; }
        public float DangerousAttacksMinuteRatio2 { get; set; }
        public float HomeDangerousAttacksMinuteRatioLog { get; set; }
        public float HomeDangerousAttacksMinuteRatio2 { get; set; }
        public float AwayDangerousAttacksMinuteRatioLog { get; set; }
        public float AwayDangerousAttacksMinuteRatio2 { get; set; }
        public float DangerousAttacksRatioLog { get; set; }
        public float DangerousAttacksRatio2 { get; set; }
        public float HomeDangerousAttacksRatioLog { get; set; }
        public float HomeDangerousAttacksRatio2 { get; set; }

        // Goal Difference
        public float GoalDiffLog { get; set; }
        public float GoalDiff2 { get; set; }

        // These are temporary fields
        public byte HomeRed { get; set; }
        public byte AwayRed { get; set; }
        public byte HomeYellow { get; set; }
        public byte AwayYellow { get; set; }
        public byte HomeOwnGoals { get; set; }
        public byte AwayOwnGoals { get; set; }
        public byte HomePossession { get; set; }
        public byte AwayPossession { get; set; }





        //public ComplexPredictionModel(LiveMatchModel match)
        //{
            
        //    List<byte> homeGoals = GoalsCalculation.GetAllGoals(match.HomeGoalMinutes);
        //    List<byte> awayGoals = GoalsCalculation.GetAllGoals(match.AwayGoalMinutes);

        //    int time = match.MatchMinutes;

        //    // goal times
        //    int timeFromLastGoal = 0, timeFromLastGoalHome = 0, timeFromLastGoalAway = 0;

        //    if (homeGoals.LastOrDefault() > awayGoals.LastOrDefault())
        //        timeFromLastGoal = match.MatchMinutes - homeGoals.LastOrDefault();
        //    else
        //        timeFromLastGoal = match.MatchMinutes - awayGoals.LastOrDefault();

        //    timeFromLastGoalHome = match.MatchMinutes - homeGoals.LastOrDefault();
        //    timeFromLastGoalAway = match.MatchMinutes - awayGoals.LastOrDefault();


        //    // critical point first 10 minutes goal
        //    bool goalScoredInTenMinutes = false;
        //    foreach (int tm in homeGoals)
        //        if (tm < 10) goalScoredInTenMinutes = true;


        //    foreach (int tm in awayGoals)
        //        if (tm < 10) goalScoredInTenMinutes = true;

        //    // shots ratios

        //    float totalShotsOnOffRatio = (match.HomeShotsOnTarget + match.AwayShotsOnTarget + match.HomeShotsOffTarget + match.AwayShotsOffTarget) == 0 ? 0 : (match.HomeShotsOnTarget + match.AwayShotsOnTarget) / (match.HomeShotsOnTarget + match.AwayShotsOnTarget + match.HomeShotsOffTarget + match.AwayShotsOffTarget);

        //    float homeShotsOnOffRatio = match.HomeShotsOnTarget + match.HomeShotsOffTarget == 0 ? 0 : (match.HomeShotsOnTarget) / (match.HomeShotsOnTarget + match.HomeShotsOffTarget);

        //    float awayShotsOnOffRatio = match.AwayShotsOnTarget + match.AwayShotsOffTarget == 0 ? 0 : (match.AwayShotsOnTarget) / (match.AwayShotsOnTarget + match.AwayShotsOffTarget);

        //    // attacks ratio
        //    float totalAttacksRatio = (match.HomeAttacks + match.AwayAttacks) == 0 ? 0 : (match.HomeDangerousAttacks + match.AwayDangerousAttacks) / (match.HomeAttacks + match.AwayAttacks);

        //    float homeAttacksRatio = match.HomeAttacks == 0 ? 0 : match.HomeDangerousAttacks / match.HomeAttacks;

        //    float awayAttackRatio = match.AwayAttacks == 0 ? 0 : match.AwayDangerousAttacks / match.AwayAttacks;


        //    // corners ratio :-

        //    float totalCornersRatio = time == 0 ? 0 : (match.HomeCorners + match.AwayCorners) / time;

        //    float homeCornersRatio = time == 0 ? 0 : (match.HomeCorners) / (time);

        //    float awayCornersRatio = time == 0 ? 0 : (match.AwayCorners) / (time);


        //    // penalties ratio
        //    float totalPenaltiesRatio = time == 0 ? 0 : (match.HomePenalties + match.AwayPenalties) / time;

        //    float homePenaltiesRatio = time == 0 ? 0 : match.HomePenalties / time;

        //    float awayPenaltiesRatio = time == 0 ? 0 : match.AwayPenalties / time;


        //    // avg time to goal ratio
        //    float avgTimeToGoal = match.HomeGoals + match.AwayGoals == 0 ? 0 : time / match.HomeGoals + match.AwayGoals;

        //    float homeAvgTimeToGoal = match.HomeGoals == 0 ? 0 : time / match.HomeGoals;
        //    float awayAvgTimeToGoal = match.AwayGoals == 0 ? 0 : time / match.AwayGoals;


        //    // Dangerous attacks ratios (one was not calculated as requirements unclear)

        //    float dangerousAttaksMinuteRatio = time == 0 ? 0 : (match.HomeDangerousAttacks + match.AwayDangerousAttacks) / 2;

        //    float homeDangerousAttacksMinuteRatio = time == 0 ? 0 : match.HomeDangerousAttacks / time;

        //    float awayDangerousAttacksMinuteRatio = time == 0 ? 0 : match.AwayDangerousAttacks / time;

        //    //DangerousAttacksRatio = ;

        //    float homeDangerousAttacksRatio = (match.HomeDangerousAttacks + match.AwayDangerousAttacks) == 0 ? 0 : match.HomeDangerousAttacks / ((match.HomeDangerousAttacks + match.AwayDangerousAttacks) / 2);

        //    float awayDangerousAttacksRatio = (match.HomeDangerousAttacks + match.AwayDangerousAttacks) == 0 ? 0 : match.AwayDangerousAttacks / ((match.HomeDangerousAttacks + match.AwayDangerousAttacks) / 2);

        //    this.HomeGoals = (byte)match.HomeGoals;
        //        HomeShotsOnTarget = (byte)match.HomeShotsOnTarget;
        //        HomeShotsOffTarget = (byte)match.HomeShotsOffTarget;
        //        HomeAttacks = (byte)match.HomeAttacks;
        //        HomeDangerousAttacks = (byte)match.HomeDangerousAttacks;
        //        HomeCorners = (byte)match.HomeCorners;
        //        HomePenalties = (byte)match.HomePenalties;
        //        AwayGoals = (byte)match.AwayGoals;
        //        AwayShotsOnTarget = (byte)match.AwayShotsOnTarget;
        //        AwayShotsOffTarget = (byte)match.AwayShotsOffTarget;
        //        AwayAttacks = (byte)match.AwayAttacks;
        //        AwayDangerousAttacks = (byte)match.AwayDangerousAttacks;
        //        AwayCorners = (byte)match.AwayCorners;
        //        AwayPenalties = (byte)match.AwayPenalties;
        //        MatchMinutes = (byte)match.MatchMinutes;
        //        TotalScore = (byte)(match.HomeGoals + match.AwayGoals);
        //        TimeFromLastGoal = (byte)timeFromLastGoal;
        //        TimeFromLastGoalHome = (byte)timeFromLastGoalHome;
        //        TimeFromLastGoalAway = (byte)timeFromLastGoalAway;
        //        CPGoalFirst10Minutes = goalScoredInTenMinutes;
        //        IsSecondHalf = match.MatchStatus == 4 ;
        //        TotalShotsOnOffRatio = totalShotsOnOffRatio;
        //        HomeShotsOnOffRatio = homeShotsOnOffRatio;
        //        AwayShotsOnOffRatio = awayShotsOnOffRatio;
        //        TotalAttacksRatio = totalAttacksRatio;
        //        HomeAttacksRatio = homeAttacksRatio;
        //        AwayAttacksRatio = awayAttackRatio;
        //        TotalCornersRatio = totalCornersRatio;
        //        HomeCornersRatio = homeCornersRatio;
        //        AwayCornersRatio = awayCornersRatio;
        //        TotalPenaltiesRatio = totalPenaltiesRatio;
        //        HomePenaltiesRatio = homePenaltiesRatio;
        //        AwayPenaltiesRatio = awayPenaltiesRatio;
        //        HomeAvgTimeToGoal = homeAvgTimeToGoal;
        //        AwayAvgTimeToGoal = awayAvgTimeToGoal;
        //        AvgTimeToGoal = avgTimeToGoal;
        //        Identity00 = match.HomeGoals == 0 && match.AwayGoals == 0 ;
        //        Identity01 = match.HomeGoals == 0 && match.AwayGoals == 1 ;
        //        Identity02 = match.HomeGoals == 0 && match.AwayGoals == 2 ;
        //        Identity03 = match.HomeGoals == 0 && match.AwayGoals == 3 ;
        //        Identity04 = match.HomeGoals == 0 && match.AwayGoals == 4 ;
        //        Identity05 = match.HomeGoals == 0 && match.AwayGoals == 5 ;
        //        Identity06 = match.HomeGoals == 0 && match.AwayGoals == 6 ;
        //        Identity07 = match.HomeGoals == 0 && match.AwayGoals == 7 ;
        //        Identity08 = match.HomeGoals == 0 && match.AwayGoals == 8 ;
        //        Identity0Over8 = match.HomeGoals == 0 && match.AwayGoals > 8 ;

        //        Identity11 = match.HomeGoals == 1 && match.AwayGoals == 1 ;
        //        Identity12 = match.HomeGoals == 1 && match.AwayGoals == 2 ;
        //        Identity13 = match.HomeGoals == 1 && match.AwayGoals == 3 ;
        //        Identity14 = match.HomeGoals == 1 && match.AwayGoals == 4 ;
        //        Identity15 = match.HomeGoals == 1 && match.AwayGoals == 5 ;
        //        Identity16 = match.HomeGoals == 1 && match.AwayGoals == 6 ;
        //        Identity17 = match.HomeGoals == 1 && match.AwayGoals == 7 ;
        //        Identity18 = match.HomeGoals == 1 && match.AwayGoals == 8 ;
        //        Identity1Over8 = match.HomeGoals == 1 && match.AwayGoals > 8 ;

        //        Identity22 = match.HomeGoals == 2 && match.AwayGoals == 2 ;
        //        Identity23 = match.HomeGoals == 2 && match.AwayGoals == 3 ;
        //        Identity24 = match.HomeGoals == 2 && match.AwayGoals == 4 ;
        //        Identity25 = match.HomeGoals == 2 && match.AwayGoals == 5 ;
        //        Identity26 = match.HomeGoals == 2 && match.AwayGoals == 6 ;
        //        Identity27 = match.HomeGoals == 2 && match.AwayGoals == 7 ;
        //        Identity28 = match.HomeGoals == 2 && match.AwayGoals == 8 ;
        //        Identity2Over8 = match.HomeGoals == 2 && match.AwayGoals > 8 ;

        //        Identity33 = match.HomeGoals == 3 && match.AwayGoals == 3 ;
        //        Identity34 = match.HomeGoals == 3 && match.AwayGoals == 4 ;
        //        Identity35 = match.HomeGoals == 3 && match.AwayGoals == 5 ;
        //        Identity36 = match.HomeGoals == 3 && match.AwayGoals == 6 ;
        //        Identity37 = match.HomeGoals == 3 && match.AwayGoals == 7 ;
        //        Identity38 = match.HomeGoals == 3 && match.AwayGoals == 8 ;
        //        Identity3Over8 = match.HomeGoals == 3 && match.AwayGoals > 8 ;

        //        Identity44 = match.HomeGoals == 4 && match.AwayGoals == 4 ;
        //        Identity45 = match.HomeGoals == 4 && match.AwayGoals == 5 ;
        //        Identity46 = match.HomeGoals == 4 && match.AwayGoals == 6 ;
        //        Identity47 = match.HomeGoals == 4 && match.AwayGoals == 7 ;
        //        Identity48 = match.HomeGoals == 4 && match.AwayGoals == 8 ;
        //        Identity4Over8 = match.HomeGoals == 4 && match.AwayGoals > 8 ;

        //        Identity55 = match.HomeGoals == 5 && match.AwayGoals == 5 ;
        //        Identity56 = match.HomeGoals == 5 && match.AwayGoals == 6 ;
        //        Identity57 = match.HomeGoals == 5 && match.AwayGoals == 7 ;
        //        Identity58 = match.HomeGoals == 5 && match.AwayGoals == 8 ;
        //        Identity5Over8 = match.HomeGoals == 5 && match.AwayGoals > 8 ;

        //        Identity66 = match.HomeGoals == 6 && match.AwayGoals == 6 ;
        //        Identity67 = match.HomeGoals == 6 && match.AwayGoals == 7 ;
        //        Identity68 = match.HomeGoals == 6 && match.AwayGoals == 8 ;
        //        Identity6Over8 = match.HomeGoals == 6 && match.AwayGoals > 8 ;

        //        Identity77 = match.HomeGoals == 7 && match.AwayGoals == 7 ;
        //        Identity78 = match.HomeGoals == 7 && match.AwayGoals == 8 ;
        //        Identity7Over8 = match.HomeGoals == 7 && match.AwayGoals > 8 ;

        //        Identity88 = match.HomeGoals == 8 && match.AwayGoals == 8 ;
        //        IdentityOver8Over8 = match.HomeGoals > 8 && match.AwayGoals > 8 ;

        //        DangerousAttacksMinuteRatio = dangerousAttaksMinuteRatio;
        //        HomeDangerousAttacksMinuteRatio = homeDangerousAttacksMinuteRatio;
        //        AwayDangerousAttacksMinuteRatio = awayDangerousAttacksMinuteRatio;
        //        //DangerousAttacksRatio = ;
        //        HomeDangerousAttacksRatio = homeDangerousAttacksRatio;
        //        AwayDangerouAttacksRatio = awayDangerousAttacksRatio;

        //        GoalDifference = (byte)(match.HomeGoals - match.AwayGoals > 0 ? match.HomeGoals - match.AwayGoals : match.AwayGoals - match.HomeGoals);
        //        Time2 = (short)(match.MatchMinutes * match.MatchMinutes);
        //        TimeLog = (float)Math.Log(match.MatchMinutes);
        //        Time3 = match.MatchMinutes * match.MatchMinutes * match.MatchMinutes;
        //        TotalAttacksRatioLog = (float)Math.Log(totalAttacksRatio);
        //        TotalAttacksRatio2 = totalAttacksRatio * totalAttacksRatio;
        //        HomeAttacksRatioLog = (float)Math.Log(homeAttacksRatio);
        //        HomeAttacksRatio2 = homeAttacksRatio * homeAttacksRatio;
        //        AwayAttacksRatioLog = (float)Math.Log(awayAttackRatio);
        //        AwayAttacksRatio2 = awayAttackRatio * awayAttackRatio;
        //        TotalPenaltiesRatioLog = (float)Math.Log(totalPenaltiesRatio);
        //        TotalPenaltiesRatio2 = totalPenaltiesRatio * totalPenaltiesRatio;
        //        HomePenaltiesRatioLog = (float)Math.Log(homePenaltiesRatio);
        //        HomePenaltiesRatio2 = homePenaltiesRatio * homePenaltiesRatio;
        //        AwayPenaltiesRatioLog = (float)Math.Log(awayPenaltiesRatio);
        //        AwayPenaltiesRatio2 = awayPenaltiesRatio * awayPenaltiesRatio;
        //        TotalCornersRatioLog = (float)Math.Log(totalCornersRatio);
        //        TotalCornersRatio2 = totalCornersRatio * totalCornersRatio;
        //        HomeCornersRatioLog = (float)Math.Log(homeCornersRatio);
        //        HomeCornersRatio2 = homeCornersRatio * homeCornersRatio;
        //        AwayCornersRatioLog = (float)Math.Log(awayCornersRatio);
        //        AwayCornersRatio2 = awayCornersRatio * awayCornersRatio;
        //        DangerousAttacksMinuteRatioLog = (float)Math.Log(dangerousAttaksMinuteRatio);
        //        DangerousAttacksMinuteRatio2 = dangerousAttaksMinuteRatio * dangerousAttaksMinuteRatio;
        //        HomeDangerousAttacksMinuteRatioLog = (float)Math.Log(homeDangerousAttacksMinuteRatio);
        //        HomeDangerousAttacksMinuteRatio2 = homeDangerousAttacksMinuteRatio * homeDangerousAttacksMinuteRatio;
        //        AwayDangerousAttacksMinuteRatioLog = (float)Math.Log(match.AwayDangerousAttacks);
        //        AwayDangerousAttacksMinuteRatio2 = match.AwayDangerousAttacks * match.AwayDangerousAttacks;
        //        DangerousAttacksRatioLog = (float)Math.Log(dangerousAttaksMinuteRatio);
        //        DangerousAttacksRatio2 = dangerousAttaksMinuteRatio * dangerousAttaksMinuteRatio;
        //        HomeDangerousAttacksRatioLog = (float)Math.Log(homeDangerousAttacksRatio);
        //        HomeDangerousAttacksRatio2 = homeDangerousAttacksRatio * homeDangerousAttacksRatio;
        //        GoalDiffLog = (float)Math.Log(match.HomeGoals - match.AwayGoals > 0 ? match.HomeGoals - match.AwayGoals : match.AwayGoals - match.HomeGoals);
        //        GoalDiff2 = (match.HomeGoals - match.AwayGoals) * (match.HomeGoals - match.AwayGoals);
        //    }
    }
}
