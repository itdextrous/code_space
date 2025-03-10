using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class RecentMatchesComebackFieldsRemove07161356 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayScoredFirstAndWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamComebackToDraw",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamComebackToWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeScoredFirstAndWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamComebackToDraw",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamComebackToWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "MatchStartTime",
                table: "RecentMatches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AwayScoredFirstAndWin",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AwayTeamComebackToDraw",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AwayTeamComebackToWin",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HomeScoredFirstAndWin",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HomeTeamComebackToDraw",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HomeTeamComebackToWin",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "MatchStartTime",
                table: "RecentMatches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
