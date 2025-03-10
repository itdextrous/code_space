using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class AddingCompetionCatogry221239 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetionCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetionCategoryCompetition",
                columns: table => new
                {
                    CompetitionCategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetionCategoryCompetition", x => new { x.CompetitionCategoriesId, x.CompetitionsId });
                    table.ForeignKey(
                        name: "FK_CompetionCategoryCompetition_CompetionCategories_CompetitionCategoriesId",
                        column: x => x.CompetitionCategoriesId,
                        principalTable: "CompetionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetionCategoryCompetition_Competitions_CompetitionsId",
                        column: x => x.CompetitionsId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetionCategoryCompetition_CompetitionsId",
                table: "CompetionCategoryCompetition",
                column: "CompetitionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetionCategoryCompetition");

            migrationBuilder.DropTable(
                name: "CompetionCategories");
        }
    }
}
