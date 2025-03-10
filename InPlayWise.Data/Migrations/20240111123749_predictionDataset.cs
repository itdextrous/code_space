using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class predictionDataset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayCorners = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayDangerousAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayGoals = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayPenalties = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayPossession = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayRed = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayShotsOffTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayShotsOnTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayYellow = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeCorners = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeDangerousAttacks = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeGoals = table.Column<byte>(type: "tinyint", nullable: false),
                    HomePenalties = table.Column<byte>(type: "tinyint", nullable: false),
                    HomePossession = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeRed = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeShotsOffTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeShotsOnTarget = table.Column<byte>(type: "tinyint", nullable: false),
                    HomeYellow = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchMinutes = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchTimeSeconds = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchData", x => x.Id);
                });
        }
    }
}
