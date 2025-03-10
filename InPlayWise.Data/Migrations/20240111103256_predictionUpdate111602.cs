using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class predictionUpdate111602 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DangerousAttacksMinuteRation",
                table: "DailyPredictionData",
                newName: "DangerousAttacksMinuteRatio");

            migrationBuilder.RenameColumn(
                name: "AwayDangerousAttacksMinuteRation",
                table: "DailyPredictionData",
                newName: "AwayDangerousAttacksMinuteRatio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DangerousAttacksMinuteRatio",
                table: "DailyPredictionData",
                newName: "DangerousAttacksMinuteRation");

            migrationBuilder.RenameColumn(
                name: "AwayDangerousAttacksMinuteRatio",
                table: "DailyPredictionData",
                newName: "AwayDangerousAttacksMinuteRation");
        }
    }
}
