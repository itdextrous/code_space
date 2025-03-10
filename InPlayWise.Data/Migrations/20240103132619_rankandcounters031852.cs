using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class rankandcounters031852 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCounters_Profiles_UserId",
                table: "FeatureCounters");

            migrationBuilder.DropTable(
                name: "LiveInsightsPerGameCounters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCounters",
                table: "FeatureCounters");

            migrationBuilder.RenameTable(
                name: "FeatureCounters",
                newName: "FeatureCounter");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamRank",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamRank",
                table: "LiveMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCounter",
                table: "FeatureCounter",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "LiveInsightsPerGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Hits = table.Column<int>(type: "int", nullable: false),
                    FeatureCounterUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveInsightsPerGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveInsightsPerGame_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LiveInsightsPerGame_FeatureCounter_FeatureCounterUserId",
                        column: x => x.FeatureCounterUserId,
                        principalTable: "FeatureCounter",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_LiveInsightsPerGame_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGame_FeatureCounterUserId",
                table: "LiveInsightsPerGame",
                column: "FeatureCounterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGame_MatchId",
                table: "LiveInsightsPerGame",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGame_UserId",
                table: "LiveInsightsPerGame",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCounter_Profiles_UserId",
                table: "FeatureCounter",
                column: "UserId",
                principalTable: "Profiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCounter_Profiles_UserId",
                table: "FeatureCounter");

            migrationBuilder.DropTable(
                name: "LiveInsightsPerGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCounter",
                table: "FeatureCounter");

            migrationBuilder.DropColumn(
                name: "AwayTeamRank",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamRank",
                table: "LiveMatches");

            migrationBuilder.RenameTable(
                name: "FeatureCounter",
                newName: "FeatureCounters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCounters",
                table: "FeatureCounters",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "LiveInsightsPerGameCounters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeatureCounterUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApiHits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveInsightsPerGameCounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveInsightsPerGameCounters_FeatureCounters_FeatureCounterUserId",
                        column: x => x.FeatureCounterUserId,
                        principalTable: "FeatureCounters",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_LiveInsightsPerGameCounters_LiveMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "LiveMatches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGameCounters_FeatureCounterUserId",
                table: "LiveInsightsPerGameCounters",
                column: "FeatureCounterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveInsightsPerGameCounters_MatchId",
                table: "LiveInsightsPerGameCounters",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCounters_Profiles_UserId",
                table: "FeatureCounters",
                column: "UserId",
                principalTable: "Profiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
