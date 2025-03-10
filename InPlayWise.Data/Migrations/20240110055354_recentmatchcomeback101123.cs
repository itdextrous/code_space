using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class recentmatchcomeback101123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AwayScoredFirstAndWin",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayScoredFirstAndWin",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeScoredFirstAndWin",
                table: "RecentMatches");
        }
    }
}
