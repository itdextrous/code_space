using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class featureCounterapihits1526 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiHitsRemaining",
                table: "LiveInsightsPerGameCounters",
                newName: "ApiHits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiHits",
                table: "LiveInsightsPerGameCounters",
                newName: "ApiHitsRemaining");
        }
    }
}
