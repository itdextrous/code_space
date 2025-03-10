using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class matchTime3011231013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayCornerMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AwayOwnGoals",
                table: "RecentMatches",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "AwayPenalties",
                table: "RecentMatches",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "AwayPossession",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AwayRedMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayShotsMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayShotsOnTargetMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwaySubstitutionMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayYellowMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeCornerMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "HomeOwnGoals",
                table: "RecentMatches",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "HomePenalties",
                table: "RecentMatches",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "HomePossession",
                table: "RecentMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HomeRedMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeShotsMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeShotsOnTargetMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeSubstitutionMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeYellowMinutes",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayCornerMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayOwnGoals",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayPenalties",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayPossession",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayRedMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayShotsMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayShotsOnTargetMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwaySubstitutionMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayYellowMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeCornerMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeOwnGoals",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomePenalties",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomePossession",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeRedMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeShotsMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeShotsOnTargetMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeSubstitutionMinutes",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeYellowMinutes",
                table: "RecentMatches");
        }
    }
}
