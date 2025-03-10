using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class featuresUpdate2258 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeageStatistics",
                table: "PlanFeatures",
                newName: "ShockDetectors");

            migrationBuilder.AddColumn<int>(
                name: "LeagueStatistics",
                table: "PlanFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueStatistics",
                table: "PlanFeatures");

            migrationBuilder.RenameColumn(
                name: "ShockDetectors",
                table: "PlanFeatures",
                newName: "LeageStatistics");
        }
    }
}
