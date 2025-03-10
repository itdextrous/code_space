using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class predictiontempfields021820 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayOwnGoals",
                table: "DailyPredictionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayRed",
                table: "DailyPredictionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayYellow",
                table: "DailyPredictionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeOwnGoals",
                table: "DailyPredictionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeRed",
                table: "DailyPredictionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeYellow",
                table: "DailyPredictionData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayOwnGoals",
                table: "DailyPredictionData");

            migrationBuilder.DropColumn(
                name: "AwayRed",
                table: "DailyPredictionData");

            migrationBuilder.DropColumn(
                name: "AwayYellow",
                table: "DailyPredictionData");

            migrationBuilder.DropColumn(
                name: "HomeOwnGoals",
                table: "DailyPredictionData");

            migrationBuilder.DropColumn(
                name: "HomeRed",
                table: "DailyPredictionData");

            migrationBuilder.DropColumn(
                name: "HomeYellow",
                table: "DailyPredictionData");
        }
    }
}
