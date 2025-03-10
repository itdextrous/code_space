using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class LeagueStatsNewFields2803 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvgCardsRed",
                table: "LeagueStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvgCardsYellow",
                table: "LeagueStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Draws",
                table: "LeagueStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverThreePointFive",
                table: "LeagueStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SeasonEndDate",
                table: "LeagueStats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SeasonStartDate",
                table: "LeagueStats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgCardsRed",
                table: "LeagueStats");

            migrationBuilder.DropColumn(
                name: "AvgCardsYellow",
                table: "LeagueStats");

            migrationBuilder.DropColumn(
                name: "Draws",
                table: "LeagueStats");

            migrationBuilder.DropColumn(
                name: "OverThreePointFive",
                table: "LeagueStats");

            migrationBuilder.DropColumn(
                name: "SeasonEndDate",
                table: "LeagueStats");

            migrationBuilder.DropColumn(
                name: "SeasonStartDate",
                table: "LeagueStats");
        }
    }
}
