using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class MatchEndInfo10090101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchEndMinutes",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchEndStatus",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchEndMinutes",
                table: "Accumulaters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchEndMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "MatchEndStatus",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "MatchEndMinutes",
                table: "Accumulaters");
        }
    }
}
