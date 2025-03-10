using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class productFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PlanFeatures_FeatureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FeatureId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "PlanFeatures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeatures_ProductId",
                table: "PlanFeatures",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFeatures_Products_ProductId",
                table: "PlanFeatures",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFeatures_Products_ProductId",
                table: "PlanFeatures");

            migrationBuilder.DropIndex(
                name: "IX_PlanFeatures_ProductId",
                table: "PlanFeatures");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PlanFeatures");

            migrationBuilder.AddColumn<Guid>(
                name: "FeatureId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
