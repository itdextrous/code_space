using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class statsAndCardNames100631 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchEndStatus",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "MatchStartTimeFormatted",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "MatchTimeSeconds",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "matchEndMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "stoppageTime",
                table: "LiveMatches");

            migrationBuilder.RenameColumn(
                name: "AbruptEnd",
                table: "LiveMatches",
                newName: "StatsComplete");

            migrationBuilder.AddColumn<string>(
                name: "AwayRedNames",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwaySubstituteNames",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayYellowNames",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeRedNames",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeSubstituteNames",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeYellowNames",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnGoalMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayRedNames",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwaySubstituteNames",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayYellowNames",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeRedNames",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeSubstituteNames",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeYellowNames",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "OwnGoalMinutes",
                table: "LiveMatches");

            migrationBuilder.RenameColumn(
                name: "StatsComplete",
                table: "LiveMatches",
                newName: "AbruptEnd");

            migrationBuilder.AddColumn<int>(
                name: "MatchEndStatus",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchStartTimeFormatted",
                table: "LiveMatches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "MatchTimeSeconds",
                table: "LiveMatches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
    }
}
