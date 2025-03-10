using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class UpdatingTableNamePredictionAccuracies191312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_predictionAccuracies",
                table: "predictionAccuracies");

            migrationBuilder.RenameTable(
                name: "predictionAccuracies",
                newName: "PredictionAccuracies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredictionAccuracies",
                table: "PredictionAccuracies",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PredictionAccuracies",
                table: "PredictionAccuracies");

            migrationBuilder.RenameTable(
                name: "PredictionAccuracies",
                newName: "predictionAccuracies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_predictionAccuracies",
                table: "predictionAccuracies",
                column: "Id");
        }
    }
}
