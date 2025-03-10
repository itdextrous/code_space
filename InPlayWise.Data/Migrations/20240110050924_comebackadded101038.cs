using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class comebackadded101038 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamHalfTimeScore",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamHalfTimeScore",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamComebackToDraw",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamComebackToWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamHalfTimeScore",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamComebackToDraw",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamComebackToWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamHalfTimeScore",
                table: "RecentMatches");
        }
    }
}
