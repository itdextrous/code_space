using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class removeOppsTable181526 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpportunitiesAll");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_MatchId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "Odds",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "Prediction",
                table: "Opportunities");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamFirstHalfWin",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamSecondHalfWin",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamWin",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BTTS",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamSecondHalfWin",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamWin",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamWinFirstHalfWin",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverEightPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverEightPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverFivePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverFivePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverFourPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverFourPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverNinePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverNinePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverOnePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverOnePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverSevenPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverSevenPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverSixPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverSixPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverThreePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverThreePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverTwoPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverTwoPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverZeroPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverZeroPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderEightPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderEightPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderFivePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderFivePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderFourPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderFourPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderNinePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderNinePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderOnePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderOnePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderSevenPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderSevenPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderSixPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderSixPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderThreePointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderThreePointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderTwoPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderTwoPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderZeroPointFiveCorners",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnderZeroPointFiveGoals",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Opportunity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odds = table.Column<float>(type: "real", nullable: false),
                    Prediction = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opportunity_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_MatchId",
                table: "Opportunities",
                column: "MatchId",
                unique: true,
                filter: "[MatchId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_MatchId",
                table: "Opportunity",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opportunity");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_MatchId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "AwayTeamFirstHalfWin",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "AwayTeamSecondHalfWin",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "AwayTeamWin",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "BTTS",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "HomeTeamSecondHalfWin",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "HomeTeamWin",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "HomeTeamWinFirstHalfWin",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverEightPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverEightPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverFivePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverFivePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverFourPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverFourPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverNinePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverNinePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverOnePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverOnePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverSevenPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverSevenPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverSixPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverSixPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverThreePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverThreePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverTwoPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverTwoPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverZeroPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OverZeroPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderEightPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderEightPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderFivePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderFivePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderFourPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderFourPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderNinePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderNinePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderOnePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderOnePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderSevenPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderSevenPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderSixPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderSixPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderThreePointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderThreePointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderTwoPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderTwoPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderZeroPointFiveCorners",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UnderZeroPointFiveGoals",
                table: "Opportunities");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Opportunities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Odds",
                table: "Opportunities",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Prediction",
                table: "Opportunities",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "OpportunitiesAll",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AwayTeamFirstHalfWin = table.Column<int>(type: "int", nullable: false),
                    AwayTeamSecondHalfWin = table.Column<int>(type: "int", nullable: false),
                    AwayTeamWin = table.Column<int>(type: "int", nullable: false),
                    BTTS = table.Column<int>(type: "int", nullable: false),
                    HomeTeamSecondHalfWin = table.Column<int>(type: "int", nullable: false),
                    HomeTeamWin = table.Column<int>(type: "int", nullable: false),
                    HomeTeamWinFirstHalfWin = table.Column<int>(type: "int", nullable: false),
                    OverEightPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverEightPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverFivePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverFivePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverFourPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverFourPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverNinePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverNinePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverOnePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverOnePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverSevenPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverSevenPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverSixPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverSixPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverThreePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverThreePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverTwoPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverTwoPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    OverZeroPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    OverZeroPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderEightPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderEightPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderFivePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderFivePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderFourPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderFourPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderNinePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderNinePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderOnePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderOnePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderSevenPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderSevenPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderSixPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderSixPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderThreePointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderThreePointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderTwoPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderTwoPointFiveGoals = table.Column<int>(type: "int", nullable: false),
                    UnderZeroPointFiveCorners = table.Column<int>(type: "int", nullable: false),
                    UnderZeroPointFiveGoals = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunitiesAll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpportunitiesAll_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_MatchId",
                table: "Opportunities",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunitiesAll_MatchId",
                table: "OpportunitiesAll",
                column: "MatchId",
                unique: true,
                filter: "[MatchId] IS NOT NULL");
        }
    }
}
