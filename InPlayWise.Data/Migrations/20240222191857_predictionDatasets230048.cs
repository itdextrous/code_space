using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class predictionDatasets230048 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredictionActiveMatchesDataset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HomeTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchMinutes = table.Column<int>(type: "int", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    HomeRed = table.Column<int>(type: "int", nullable: false),
                    AwayRed = table.Column<int>(type: "int", nullable: false),
                    HomeYellow = table.Column<int>(type: "int", nullable: false),
                    AwayYellow = table.Column<int>(type: "int", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    HomePenalties = table.Column<int>(type: "int", nullable: false),
                    AwayPenalties = table.Column<int>(type: "int", nullable: false),
                    HomePossession = table.Column<int>(type: "int", nullable: false),
                    AwayPossession = table.Column<int>(type: "int", nullable: false),
                    HomeOwnGoals = table.Column<int>(type: "int", nullable: false),
                    AwayOwnGoals = table.Column<int>(type: "int", nullable: false),
                    HomeTeamRank = table.Column<int>(type: "int", nullable: false),
                    AwayTeamRank = table.Column<int>(type: "int", nullable: false),
                    HomeTeamHalfTimeScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamHalfTimeScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTimeScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTimeScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTimeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTimeCorners = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionActiveMatchesDataset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredictionActiveMatchesDataset_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredictionFullDataSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchMinutes = table.Column<int>(type: "int", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    HomeRed = table.Column<int>(type: "int", nullable: false),
                    AwayRed = table.Column<int>(type: "int", nullable: false),
                    HomeYellow = table.Column<int>(type: "int", nullable: false),
                    AwayYellow = table.Column<int>(type: "int", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    HomePenalties = table.Column<int>(type: "int", nullable: false),
                    AwayPenalties = table.Column<int>(type: "int", nullable: false),
                    HomePossession = table.Column<int>(type: "int", nullable: false),
                    AwayPossession = table.Column<int>(type: "int", nullable: false),
                    HomeOwnGoals = table.Column<int>(type: "int", nullable: false),
                    AwayOwnGoals = table.Column<int>(type: "int", nullable: false),
                    HomeTeamRank = table.Column<int>(type: "int", nullable: false),
                    AwayTeamRank = table.Column<int>(type: "int", nullable: false),
                    HomeTeamHalfTimeScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamHalfTimeScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTimeScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTimeScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamFullTimeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFullTimeCorners = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionFullDataSet", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PredictionActiveMatchesDataset_MatchId",
                table: "PredictionActiveMatchesDataset",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredictionActiveMatchesDataset");

            migrationBuilder.DropTable(
                name: "PredictionFullDataSet");
        }
    }
}
