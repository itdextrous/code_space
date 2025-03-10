using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class LiveInsightsCounter1040 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGameCounters_FeatureCounters_Id",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.AddColumn<string>(
                name: "FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGameCounters_FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters",
                column: "FeatureCounterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGameCounters_FeatureCounters_FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters",
                column: "FeatureCounterUserId",
                principalTable: "FeatureCounters",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGameCounters_FeatureCounters_FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.DropIndex(
                name: "IX_LiveInsightsPerGameCounters_FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.DropColumn(
                name: "FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGameCounters_FeatureCounters_Id",
                table: "LiveInsightsPerGameCounters",
                column: "Id",
                principalTable: "FeatureCounters",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
