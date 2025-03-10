using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class matchGoalMinutesStoppage100124 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayTeamHalfTimeScore",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamOverTimeScore",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamPenaltyShootoutScore",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamHalfTimeScore",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamOverTimeScore",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamPenaltyShootOutScore",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamHalfTimeScore",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamOverTimeScore",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamPenaltyShootoutScore",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamHalfTimeScore",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamOverTimeScore",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamPenaltyShootOutScore",
                table: "LiveMatches");
        }
    }
}
