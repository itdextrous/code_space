using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class opportunitiestable1133 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Opportunities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UnderZeroPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverZeroPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderOnePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverOnePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderTwoPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverTwoPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderThreePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverThreePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderFourPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverFourPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderFivePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverFivePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderSixPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverSixPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderSevenPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverSevenPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderEightPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverEightPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderNinePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverNinePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderZeroPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverZeroPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderOnePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverOnePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderTwoPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverTwoPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderThreePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverThreePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderFourPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverFourPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderFivePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverFivePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderSixPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverSixPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderSevenPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverSevenPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderEightPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverEightPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderNinePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverNinePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    BTTS = table.Column<int>(type: "int", nullable: false),
                    HomeTeamWin = table.Column<int>(type: "int", nullable: false),
                    AwayTeamWin = table.Column<int>(type: "int", nullable: false),
                    HomeTeamWinFirstHalfWin = table.Column<int>(type: "int", nullable: false),
                    AwayTeamFirstHalfWin = table.Column<int>(type: "int", nullable: false),
                    HomeTeamSecondHalfWin = table.Column<int>(type: "int", nullable: false),
                    AwayTeamSecondHalfWin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opportunities_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_MatchId",
                table: "Opportunities",
                column: "MatchId",
                unique: true,
                filter: "[MatchId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opportunities");
        }
    }
}
