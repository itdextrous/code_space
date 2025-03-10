using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class featureCounter1517 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatorHistory",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "CleverLabelling",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "Hedge",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "LeagueStats",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "ShockDetectors",
                table: "FeatureCounters");

            migrationBuilder.AddColumn<string>(
                name: "LiveInsightsGameId",
                table: "FeatureCounters",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCounters_LiveInsightsGameId",
                table: "FeatureCounters",
                column: "LiveInsightsGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCounters_LiveInsightsPerGameCounters_LiveInsightsGameId",
                table: "FeatureCounters",
                column: "LiveInsightsGameId",
                principalTable: "LiveInsightsPerGameCounters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCounters_LiveInsightsPerGameCounters_LiveInsightsGameId",
                table: "FeatureCounters");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCounters_LiveInsightsGameId",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "LiveInsightsGameId",
                table: "FeatureCounters");

            migrationBuilder.AddColumn<int>(
                name: "AccumulatorHistory",
                table: "FeatureCounters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CleverLabelling",
                table: "FeatureCounters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hedge",
                table: "FeatureCounters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueStats",
                table: "FeatureCounters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShockDetectors",
                table: "FeatureCounters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
