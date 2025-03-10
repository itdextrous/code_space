using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class dailypred021634 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prediction",
                table: "Prediction");

            migrationBuilder.RenameTable(
                name: "Prediction",
                newName: "DailyPredictionData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyPredictionData",
                table: "DailyPredictionData",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyPredictionData",
                table: "DailyPredictionData");

            migrationBuilder.RenameTable(
                name: "DailyPredictionData",
                newName: "Prediction");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prediction",
                table: "Prediction",
                column: "Id");
        }
    }
}
