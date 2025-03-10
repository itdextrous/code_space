using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class InsightsLive2320 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Insights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LivePossession = table.Column<int>(type: "int", nullable: false),
                    LiveDangerousAttackPercentage = table.Column<int>(type: "int", nullable: false),
                    LiveShotsOnTargetPercentage = table.Column<int>(type: "int", nullable: false),
                    LiveCorners = table.Column<int>(type: "int", nullable: false),
                    GoalsScoredAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsScoredFirstHalfAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsScoredSecondHalfAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsConcededAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsConcededFirstHalfAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsConcededSecondHalfAvg = table.Column<float>(type: "real", nullable: false),
                    ScoredFirstHalfAndSecondHalfPercent = table.Column<int>(type: "int", nullable: false),
                    GoalsConcededFirstHalfAndSecondHalfPercent = table.Column<int>(type: "int", nullable: false),
                    GoalsAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsFirstHalfAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsSecondHalfAvg = table.Column<float>(type: "real", nullable: false),
                    GoalsFirstHalfAndSecondHalfPercent = table.Column<int>(type: "int", nullable: false),
                    BothTeamsScoredPercent = table.Column<int>(type: "int", nullable: false),
                    NoGoalScoredPercent = table.Column<int>(type: "int", nullable: false),
                    HomeWinPercent = table.Column<int>(type: "int", nullable: false),
                    AwayWinPercent = table.Column<int>(type: "int", nullable: false),
                    ShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    DangerousAttack = table.Column<int>(type: "int", nullable: false),
                    ShotsOnTargetAverage = table.Column<float>(type: "real", nullable: false),
                    DangerousAttacksAverage = table.Column<float>(type: "real", nullable: false),
                    AverageCornersOfTeam = table.Column<float>(type: "real", nullable: false),
                    AverageCornersInGame = table.Column<float>(type: "real", nullable: false),
                    CleanSheetPercent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insights_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Insights_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insights_MatchId",
                table: "Insights",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Insights_TeamId",
                table: "Insights",
                column: "TeamId",
                unique: true,
                filter: "[TeamId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Insights");
        }
    }
}
