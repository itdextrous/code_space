using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class livematchRecentMatchFields1540 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfficialStartTime",
                table: "LiveMatches",
                newName: "MatchStartTimeOfficial");

            migrationBuilder.RenameColumn(
                name: "ExtraTime",
                table: "LiveMatches",
                newName: "PenaltyShootOut");

            migrationBuilder.RenameColumn(
                name: "ExtraPenalties",
                table: "LiveMatches",
                newName: "OverTime");

            migrationBuilder.RenameColumn(
                name: "EndedAbruptly",
                table: "LiveMatches",
                newName: "AbruptEnd");

            migrationBuilder.AddColumn<bool>(
                name: "AbruptEnd",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ended",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchStartTimeOfficial",
                table: "RecentMatches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "OverTime",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PenaltyShootout",
                table: "RecentMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalMatchMinutes",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "MatchEndStatus",
                table: "LiveMatches",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbruptEnd",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "Ended",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "MatchStartTimeOfficial",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "OverTime",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "PenaltyShootout",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "TotalMatchMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "MatchEndStatus",
                table: "LiveMatches");

            migrationBuilder.RenameColumn(
                name: "PenaltyShootOut",
                table: "LiveMatches",
                newName: "ExtraTime");

            migrationBuilder.RenameColumn(
                name: "OverTime",
                table: "LiveMatches",
                newName: "ExtraPenalties");

            migrationBuilder.RenameColumn(
                name: "MatchStartTimeOfficial",
                table: "LiveMatches",
                newName: "OfficialStartTime");

            migrationBuilder.RenameColumn(
                name: "AbruptEnd",
                table: "LiveMatches",
                newName: "EndedAbruptly");
        }
    }
}
