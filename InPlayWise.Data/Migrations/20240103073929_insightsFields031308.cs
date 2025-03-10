using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class insightsFields031308 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OverOnePointFivePercent",
                table: "Insights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverThreePointFivePercent",
                table: "Insights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverTwoPointFivePercent",
                table: "Insights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverZeroPointFivePercent",
                table: "Insights",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverOnePointFivePercent",
                table: "Insights");

            migrationBuilder.DropColumn(
                name: "OverThreePointFivePercent",
                table: "Insights");

            migrationBuilder.DropColumn(
                name: "OverTwoPointFivePercent",
                table: "Insights");

            migrationBuilder.DropColumn(
                name: "OverZeroPointFivePercent",
                table: "Insights");
        }
    }
}
