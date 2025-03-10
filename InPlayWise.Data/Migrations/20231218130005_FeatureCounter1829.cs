using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class FeatureCounter1829 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "MatchId",
                table: "LiveInsightsPerGameCounters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGameCounters_MatchId",
                table: "LiveInsightsPerGameCounters",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGameCounters_FeatureCounters_Id",
                table: "LiveInsightsPerGameCounters",
                column: "Id",
                principalTable: "FeatureCounters",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGameCounters_LiveMatches_MatchId",
                table: "LiveInsightsPerGameCounters",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGameCounters_FeatureCounters_Id",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGameCounters_LiveMatches_MatchId",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.DropIndex(
                name: "IX_LiveInsightsPerGameCounters_MatchId",
                table: "LiveInsightsPerGameCounters");

            migrationBuilder.AlterColumn<string>(
                name: "MatchId",
                table: "LiveInsightsPerGameCounters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
