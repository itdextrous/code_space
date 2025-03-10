using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class productFeature0932 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Products_FeatureId",
                table: "Products",
                column: "FeatureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PlanFeatures_FeatureId",
                table: "Products",
                column: "FeatureId",
                principalTable: "PlanFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PlanFeatures_FeatureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FeatureId",
                table: "Products");

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
    }
}
