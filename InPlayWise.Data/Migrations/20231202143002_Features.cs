using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class Features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeatureId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PlanFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LiveInsightPerGame = table.Column<int>(type: "int", nullable: false),
                    LivePredictionPerGAme = table.Column<int>(type: "int", nullable: false),
                    MaxPredictions = table.Column<int>(type: "int", nullable: false),
                    AccumulatorGenerators = table.Column<int>(type: "int", nullable: false),
                    CleverLabelling = table.Column<int>(type: "int", nullable: false),
                    HistoryOfAccumulators = table.Column<int>(type: "int", nullable: false),
                    WiseProHedge = table.Column<int>(type: "int", nullable: false),
                    LeageStatistics = table.Column<int>(type: "int", nullable: false),
                    WiseProIncluded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanFeatures_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanFeatures");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Products");
        }
    }
}
