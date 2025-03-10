using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class FeatureCounters1137 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureCounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccumulatorGenerators = table.Column<int>(type: "int", nullable: false),
                    ShockDetectors = table.Column<int>(type: "int", nullable: false),
                    CleverLabelling = table.Column<int>(type: "int", nullable: false),
                    AccumulatorHistory = table.Column<int>(type: "int", nullable: false),
                    Hedge = table.Column<int>(type: "int", nullable: false),
                    LeagueStats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCounters_Profiles_UserId",
                        column: x => x.UserId,
                        principalTable: "Profiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCounters_UserId",
                table: "FeatureCounters",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCounters");
        }
    }
}
