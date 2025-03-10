using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class RedundantTablesRemove05120001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PredictionActiveMatchesDataset_LiveMatches_MatchId",
                table: "PredictionActiveMatchesDataset");

            migrationBuilder.DropTable(
                name: "AllPredictionData");

            migrationBuilder.DropTable(
                name: "AllPredictionDataset");

            migrationBuilder.DropTable(
                name: "DailyPredictionData");

            migrationBuilder.DropTable(
                name: "DailyPredictionDataset");

            migrationBuilder.DropTable(
                name: "PredictionFullDataSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PredictionActiveMatchesDataset",
                table: "PredictionActiveMatchesDataset");

            migrationBuilder.RenameTable(
                name: "PredictionActiveMatchesDataset",
                newName: "PredictionActiveMatchesData");

            migrationBuilder.RenameIndex(
                name: "IX_PredictionActiveMatchesDataset_MatchId",
                table: "PredictionActiveMatchesData",
                newName: "IX_PredictionActiveMatchesData_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredictionActiveMatchesData",
                table: "PredictionActiveMatchesData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PredictionActiveMatchesData_LiveMatches_MatchId",
                table: "PredictionActiveMatchesData",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PredictionActiveMatchesData_LiveMatches_MatchId",
                table: "PredictionActiveMatchesData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PredictionActiveMatchesData",
                table: "PredictionActiveMatchesData");

            migrationBuilder.RenameTable(
                name: "PredictionActiveMatchesData",
                newName: "PredictionActiveMatchesDataset");

            migrationBuilder.RenameIndex(
                name: "IX_PredictionActiveMatchesData_MatchId",
                table: "PredictionActiveMatchesDataset",
                newName: "IX_PredictionActiveMatchesDataset_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredictionActiveMatchesDataset",
                table: "PredictionActiveMatchesDataset",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AllPredictionData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayAvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    AwayCornersRatio = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayDangerouAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayDangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayDangerousAttacksMinuteRation = table.Column<float>(type: "real", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    AwayPenalties = table.Column<int>(type: "int", nullable: false),
                    AwayPenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    CPGoalFirst10Minutes = table.Column<int>(type: "int", nullable: false),
                    DangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksMinuteRation = table.Column<float>(type: "real", nullable: false),
                    DangerousAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    DangerousAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    GoalDiff2 = table.Column<double>(type: "float", nullable: false),
                    GoalDiffLog = table.Column<double>(type: "float", nullable: false),
                    GoalDifference = table.Column<int>(type: "int", nullable: false),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeAvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    HomeCornersRatio = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacksMinuteRatio = table.Column<float>(type: "real", nullable: false),
                    HomeDangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    HomeDangerousAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    HomePenalties = table.Column<int>(type: "int", nullable: false),
                    HomePenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    Identity00 = table.Column<int>(type: "int", nullable: false),
                    Identity01 = table.Column<int>(type: "int", nullable: false),
                    Identity02 = table.Column<int>(type: "int", nullable: false),
                    Identity03 = table.Column<int>(type: "int", nullable: false),
                    Identity04 = table.Column<int>(type: "int", nullable: false),
                    Identity05 = table.Column<int>(type: "int", nullable: false),
                    Identity06 = table.Column<int>(type: "int", nullable: false),
                    Identity07 = table.Column<int>(type: "int", nullable: false),
                    Identity08 = table.Column<int>(type: "int", nullable: false),
                    Identity0Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity11 = table.Column<int>(type: "int", nullable: false),
                    Identity12 = table.Column<int>(type: "int", nullable: false),
                    Identity13 = table.Column<int>(type: "int", nullable: false),
                    Identity14 = table.Column<int>(type: "int", nullable: false),
                    Identity15 = table.Column<int>(type: "int", nullable: false),
                    Identity16 = table.Column<int>(type: "int", nullable: false),
                    Identity17 = table.Column<int>(type: "int", nullable: false),
                    Identity18 = table.Column<int>(type: "int", nullable: false),
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
                    IsSecondHalf = table.Column<bool>(type: "bit", nullable: false),
                    MatchMinutes = table.Column<int>(type: "int", nullable: false),
                    Time2 = table.Column<int>(type: "int", nullable: false),
                    Time3 = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoal = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoalAway = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoalHome = table.Column<int>(type: "int", nullable: false),
                    TimeLog = table.Column<float>(type: "real", nullable: false),
                    TotalAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    TotalAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatio = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    TotalShotsOnOffRatio = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPredictionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllPredictionDataset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayCorners = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayDangerousAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayGoals = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayPenalties = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayPossession = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayRed = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayShotsOffTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayShotsOnTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayYellow = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeCorners = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeDangerousAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeGoals = table.Column<byte>(type: "tinyint", nullable: false),
                    HomePenalties = table.Column<byte>(type: "tinyint", nullable: false),
                    HomePossession = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeRed = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeShotsOffTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeShotsOnTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeYellow = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchMinutes = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchTimeSeconds = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPredictionDataset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPredictionData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayAvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    AwayCornersRatio = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayDangerouAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacksMinuteRatio = table.Column<float>(type: "real", nullable: false),
                    AwayDangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayDangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    AwayOwnGoals = table.Column<int>(type: "int", nullable: false),
                    AwayPenalties = table.Column<int>(type: "int", nullable: false),
                    AwayPenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    AwayPenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    AwayPossession = table.Column<int>(type: "int", nullable: false),
                    AwayRed = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayYellow = table.Column<int>(type: "int", nullable: false),
                    CPGoalFirst10Minutes = table.Column<byte>(type: "tinyint", nullable: false),
                    DangerousAttacksMinuteRatio = table.Column<float>(type: "real", nullable: false),
                    DangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    DangerousAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    DangerousAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    GoalDiff2 = table.Column<double>(type: "float", nullable: false),
                    GoalDiffLog = table.Column<double>(type: "float", nullable: false),
                    GoalDifference = table.Column<int>(type: "int", nullable: false),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeAvgTimeToGoal = table.Column<float>(type: "real", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    HomeCornersRatio = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacksMinuteRatio = table.Column<float>(type: "real", nullable: false),
                    HomeDangerousAttacksMinuteRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksMinuteRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksRatio = table.Column<float>(type: "real", nullable: false),
                    HomeDangerousAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomeDangerousAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    HomeOwnGoals = table.Column<int>(type: "int", nullable: false),
                    HomePenalties = table.Column<int>(type: "int", nullable: false),
                    HomePenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    HomePenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    HomePossession = table.Column<int>(type: "int", nullable: false),
                    HomeRed = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOnOffRatio = table.Column<double>(type: "float", nullable: false),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomeYellow = table.Column<int>(type: "int", nullable: false),
                    Identity00 = table.Column<int>(type: "int", nullable: false),
                    Identity01 = table.Column<int>(type: "int", nullable: false),
                    Identity02 = table.Column<int>(type: "int", nullable: false),
                    Identity03 = table.Column<int>(type: "int", nullable: false),
                    Identity04 = table.Column<int>(type: "int", nullable: false),
                    Identity05 = table.Column<int>(type: "int", nullable: false),
                    Identity06 = table.Column<int>(type: "int", nullable: false),
                    Identity07 = table.Column<int>(type: "int", nullable: false),
                    Identity08 = table.Column<int>(type: "int", nullable: false),
                    Identity0Over8 = table.Column<int>(type: "int", nullable: false),
                    Identity11 = table.Column<int>(type: "int", nullable: false),
                    Identity12 = table.Column<int>(type: "int", nullable: false),
                    Identity13 = table.Column<int>(type: "int", nullable: false),
                    Identity14 = table.Column<int>(type: "int", nullable: false),
                    Identity15 = table.Column<int>(type: "int", nullable: false),
                    Identity16 = table.Column<int>(type: "int", nullable: false),
                    Identity17 = table.Column<int>(type: "int", nullable: false),
                    Identity18 = table.Column<int>(type: "int", nullable: false),
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
                    IsSecondHalf = table.Column<bool>(type: "bit", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchMinutes = table.Column<int>(type: "int", nullable: false),
                    Time2 = table.Column<int>(type: "int", nullable: false),
                    Time3 = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoal = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoalAway = table.Column<int>(type: "int", nullable: false),
                    TimeFromLastGoalHome = table.Column<int>(type: "int", nullable: false),
                    TimeLog = table.Column<float>(type: "real", nullable: false),
                    TotalAttacksRatio = table.Column<double>(type: "float", nullable: false),
                    TotalAttacksRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalAttacksRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatio = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalCornersRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatio = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatio2 = table.Column<double>(type: "float", nullable: false),
                    TotalPenaltiesRatioLog = table.Column<double>(type: "float", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    TotalShotsOnOffRatio = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPredictionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPredictionDataset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayCorners = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayDangerousAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayGoals = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayPenalties = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayPossession = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayRed = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayShotsOffTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayShotsOnTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayYellow = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeCorners = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeDangerousAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeGoals = table.Column<byte>(type: "tinyint", nullable: false),
                    HomePenalties = table.Column<byte>(type: "tinyint", nullable: false),
                    HomePossession = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeRed = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeShotsOffTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeShotsOnTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeYellow = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchMinutes = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchTimeSeconds = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPredictionDataset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PredictionFullDataSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    AwayOwnGoals = table.Column<int>(type: "int", nullable: false),
                    AwayPenalties = table.Column<int>(type: "int", nullable: false),
                    AwayPossession = table.Column<int>(type: "int", nullable: false),
                    AwayRed = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTimeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTimeScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamHalfTimeScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamRank = table.Column<int>(type: "int", nullable: false),
                    AwayYellow = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    HomeOwnGoals = table.Column<int>(type: "int", nullable: false),
                    HomePenalties = table.Column<int>(type: "int", nullable: false),
                    HomePossession = table.Column<int>(type: "int", nullable: false),
                    HomeRed = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTimeCorners = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTimeScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamHalfTimeScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamRank = table.Column<int>(type: "int", nullable: false),
                    HomeYellow = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchMinutes = table.Column<int>(type: "int", nullable: false),
                    MatchTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionFullDataSet", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PredictionActiveMatchesDataset_LiveMatches_MatchId",
                table: "PredictionActiveMatchesDataset",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
