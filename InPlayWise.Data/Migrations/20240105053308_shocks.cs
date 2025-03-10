using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class shocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                table: "LiveInsightsPerGame");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGame_FeatureCounter_FeatureCounterUserId",
                table: "LiveInsightsPerGame");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsPerGame_LiveMatches_MatchId",
                table: "LiveInsightsPerGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiveInsightsPerGame",
                table: "LiveInsightsPerGame");

            migrationBuilder.RenameTable(
                name: "LiveInsightsPerGame",
                newName: "LiveInsightsCounter");

            migrationBuilder.RenameIndex(
                name: "IX_LiveInsightsPerGame_UserId",
                table: "LiveInsightsCounter",
                newName: "IX_LiveInsightsCounter_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveInsightsPerGame_MatchId",
                table: "LiveInsightsCounter",
                newName: "IX_LiveInsightsCounter_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveInsightsPerGame_FeatureCounterUserId",
                table: "LiveInsightsCounter",
                newName: "IX_LiveInsightsCounter_FeatureCounterUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiveInsightsCounter",
                table: "LiveInsightsCounter",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Shocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shocks_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId");
                    table.ForeignKey(
                        name: "FK_Shocks_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shocks_MatchId",
                table: "Shocks",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Shocks_TeamId",
                table: "Shocks",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsCounter_AspNetUsers_UserId",
                table: "LiveInsightsCounter",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsCounter_FeatureCounter_FeatureCounterUserId",
                table: "LiveInsightsCounter",
                column: "FeatureCounterUserId",
                principalTable: "FeatureCounter",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsCounter_LiveMatches_MatchId",
                table: "LiveInsightsCounter",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsCounter_AspNetUsers_UserId",
                table: "LiveInsightsCounter");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsCounter_FeatureCounter_FeatureCounterUserId",
                table: "LiveInsightsCounter");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveInsightsCounter_LiveMatches_MatchId",
                table: "LiveInsightsCounter");

            migrationBuilder.DropTable(
                name: "Shocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiveInsightsCounter",
                table: "LiveInsightsCounter");

            migrationBuilder.RenameTable(
                name: "LiveInsightsCounter",
                newName: "LiveInsightsPerGame");

            migrationBuilder.RenameIndex(
                name: "IX_LiveInsightsCounter_UserId",
                table: "LiveInsightsPerGame",
                newName: "IX_LiveInsightsPerGame_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveInsightsCounter_MatchId",
                table: "LiveInsightsPerGame",
                newName: "IX_LiveInsightsPerGame_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveInsightsCounter_FeatureCounterUserId",
                table: "LiveInsightsPerGame",
                newName: "IX_LiveInsightsPerGame_FeatureCounterUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiveInsightsPerGame",
                table: "LiveInsightsPerGame",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                table: "LiveInsightsPerGame",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGame_FeatureCounter_FeatureCounterUserId",
                table: "LiveInsightsPerGame",
                column: "FeatureCounterUserId",
                principalTable: "FeatureCounter",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveInsightsPerGame_LiveMatches_MatchId",
                table: "LiveInsightsPerGame",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
