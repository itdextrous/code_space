using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class livematchcompetition15072249 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamLogo",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamName",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayTeamShortName",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "CompetitionLogo",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "CompetitionName",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "CompetitionShortName",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamLogo",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamName",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamShortName",
                table: "LiveMatches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayTeamLogo",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayTeamName",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayTeamShortName",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionLogo",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionName",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionShortName",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamLogo",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamName",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamShortName",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
