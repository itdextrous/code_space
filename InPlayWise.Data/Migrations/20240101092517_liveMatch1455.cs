using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class liveMatch1455 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ended",
                table: "LiveMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EndedAbruptly",
                table: "LiveMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExtraPenalties",
                table: "LiveMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExtraTime",
                table: "LiveMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OfficialStartTime",
                table: "LiveMatches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "matchEndMinutes",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "stoppageTime",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ended",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "EndedAbruptly",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "ExtraPenalties",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "ExtraTime",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "OfficialStartTime",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "matchEndMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "stoppageTime",
                table: "LiveMatches");
        }
    }
}
