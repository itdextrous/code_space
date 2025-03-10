using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class ShockCounter051154 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shocks_LiveMatches_MatchId",
                table: "Shocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Shocks_Team_TeamId",
                table: "Shocks");

            migrationBuilder.CreateTable(
                name: "ShockCounter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Matchid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShockCounter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShockCounter_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShockCounter_LiveMatches_Matchid",
                        column: x => x.Matchid,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShockCounter_Matchid",
                table: "ShockCounter",
                column: "Matchid");

            migrationBuilder.CreateIndex(
                name: "IX_ShockCounter_UserId",
                table: "ShockCounter",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Shocks_LiveMatches_MatchId",
                table: "Shocks",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shocks_Team_TeamId",
                table: "Shocks",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shocks_LiveMatches_MatchId",
                table: "Shocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Shocks_Team_TeamId",
                table: "Shocks");

            migrationBuilder.DropTable(
                name: "ShockCounter");

            migrationBuilder.AddForeignKey(
                name: "FK_Shocks_LiveMatches_MatchId",
                table: "Shocks",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shocks_Team_TeamId",
                table: "Shocks",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id");
        }
    }
}
