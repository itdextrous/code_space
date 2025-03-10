﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class predictiontable021751 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwayPenaltiesAwarded",
                table: "DailyPredictionData",
                newName: "AwayPenalties");

            migrationBuilder.CreateTable(
                name: "AllPredictionData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    HomePenalties = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    AwayPenalties = table.Column<int>(type: "int", nullable: false),
                    MatchMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoal = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoalHome = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoalAway = table.Column<int>(type: "int", nullable: false),
                    CPGoalFirst10Minutes = table.Column<int>(type: "int", nullable: false),
                    TotalShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    HomeShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    AwayShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    TotalAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatio = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatio = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatio = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    HomeAvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    AwayAvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    AvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    Identity00 = table.Column<int>(type: "int", nullable: false),
                    Identity01 = table.Column<int>(type: "int", nullable: false),
                    Identity02 = table.Column<int>(type: "int", nullable: false),
                    Identity03 = table.Column<int>(type: "int", nullable: false),
                    Identity04 = table.Column<int>(type: "int", nullable: false),
                    Identity05 = table.Column<int>(type: "int", nullable: false),
                    Identity06 = table.Column<int>(type: "int", nullable: false),
                    Identity07 = table.Column<int>(type: "int", nullable: false),
                    Identity08 = table.Column<int>(type: "int", nullable: false),
                    Identity11 = table.Column<int>(type: "int", nullable: false),
                    Identity12 = table.Column<int>(type: "int", nullable: false),
                    Identity13 = table.Column<int>(type: "int", nullable: false),
                    Identity14 = table.Column<int>(type: "int", nullable: false),
                    Identity15 = table.Column<int>(type: "int", nullable: false),
                    Identity16 = table.Column<int>(type: "int", nullable: false),
                    Identity17 = table.Column<int>(type: "int", nullable: false),
                    Identity18 = table.Column<int>(type: "int", nullable: false),
                    Identity0Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity1Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity22 = table.Column<int>(type: "int", nullable: false),
                    Identity23 = table.Column<int>(type: "int", nullable: false),
                    Identity24 = table.Column<int>(type: "int", nullable: false),
                    Identity25 = table.Column<int>(type: "int", nullable: false),
                    Identity26 = table.Column<int>(type: "int", nullable: false),
                    Identity27 = table.Column<int>(type: "int", nullable: false),
                    Identity28 = table.Column<int>(type: "int", nullable: false),
                    Identity2Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity33 = table.Column<int>(type: "int", nullable: false),
                    Identity34 = table.Column<int>(type: "int", nullable: false),
                    Identity35 = table.Column<int>(type: "int", nullable: false),
                    Identity36 = table.Column<int>(type: "int", nullable: false),
                    Identity37 = table.Column<int>(type: "int", nullable: false),
                    Identity38 = table.Column<int>(type: "int", nullable: false),
                    Identity3Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity44 = table.Column<int>(type: "int", nullable: false),
                    Identity45 = table.Column<int>(type: "int", nullable: false),
                    Identity46 = table.Column<int>(type: "int", nullable: false),
                    Identity47 = table.Column<int>(type: "int", nullable: false),
                    Identity48 = table.Column<int>(type: "int", nullable: false),
                    Identity4Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity55 = table.Column<int>(type: "int", nullable: false),
                    Identity56 = table.Column<int>(type: "int", nullable: false),
                    Identity57 = table.Column<int>(type: "int", nullable: false),
                    Identity58 = table.Column<int>(type: "int", nullable: false),
                    Identity5Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity66 = table.Column<int>(type: "int", nullable: false),
                    Identity67 = table.Column<int>(type: "int", nullable: false),
                    Identity68 = table.Column<int>(type: "int", nullable: false),
                    Identity6Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity77 = table.Column<int>(type: "int", nullable: false),
                    Identity78 = table.Column<int>(type: "int", nullable: false),
                    Identity7Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity88 = table.Column<int>(type: "int", nullable: false),
                    IdentityOver8Over8 = table.Column<int>(type: "int", nullable: false),
                    DangerousAttacksMinuteRation = table.Column<float>(type: "real", nullable: false),
                    HomeDangerousAttacksMinuteRatio = table.Column<float>(type: "real", nullable: false),
                    AwayDangerousAttacksMinuteRation = table.Column<float>(type: "real", nullable: false),
                    DangerousAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    HomeDangerousAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    AwayDangerouAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    IsSecondHalf = table.Column<bool>(type: "bit", nullable: false),
                    GoalDifference = table.Column<int>(type: "int", nullable: false),
                    Time2 = table.Column<int>(type: "int", nullable: false),
                    TimeLog = table.Column<float>(type: "real", nullable: false),
                    Time3 = table.Column<int>(type: "int", nullable: false),
                    TotalAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayDangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayDangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    GoalDiffLog = table.Column<double>(type: "float", nullable: false),
                    GoalDiff2 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPredictionData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllPredictionData");

            migrationBuilder.RenameColumn(
                name: "AwayPenalties",
                table: "DailyPredictionData",
                newName: "AwayPenaltiesAwarded");
        }
    }
}
