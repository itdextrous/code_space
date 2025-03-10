using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class liveInsightRel041725 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                table: "LiveInsightsPerGame");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                table: "LiveInsightsPerGame",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                table: "LiveInsightsPerGame");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                table: "LiveInsightsPerGame",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
