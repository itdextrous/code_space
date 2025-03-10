using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class RecentMatchNameRemove07160132 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamName",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "CompetitionName",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamName",
                table: "RecentMatches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayTeamName",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionName",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamName",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
