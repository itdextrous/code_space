using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class productClear0103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFeatures_Products_Id",
                table: "PlanFeatures");

            migrationBuilder.AddColumn<Guid>(
                name: "FeaturesId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FeaturesId",
                table: "Products",
                column: "FeaturesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PlanFeatures_FeaturesId",
                table: "Products",
                column: "FeaturesId",
                principalTable: "PlanFeatures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PlanFeatures_FeaturesId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FeaturesId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FeaturesId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFeatures_Products_Id",
                table: "PlanFeatures",
                column: "Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
