using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class pedictionbasicdatasets111750 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchMinutes = table.Column<short>(type: "smallint", nullable: false),
                    HomeGoals = table.Column<short>(type: "smallint", nullable: false),
                    AwayGoals = table.Column<short>(type: "smallint", nullable: false),
                    HomeRed = table.Column<short>(type: "smallint", nullable: false),
                    AwayRed = table.Column<short>(type: "smallint", nullable: false),
                    HomeYellow = table.Column<short>(type: "smallint", nullable: false),
                    AwayYellow = table.Column<short>(type: "smallint", nullable: false),
                    HomeCorners = table.Column<short>(type: "smallint", nullable: false),
                    AwayCorners = table.Column<short>(type: "smallint", nullable: false),
                    HomeShotsOnTarget = table.Column<short>(type: "smallint", nullable: false),
                    HomeShotsOffTarget = table.Column<short>(type: "smallint", nullable: false),
                    AwayShotsOnTarget = table.Column<short>(type: "smallint", nullable: false),
                    AwayShotsOffTarget = table.Column<short>(type: "smallint", nullable: false),
                    HomeDangerousAttacks = table.Column<short>(type: "smallint", nullable: false),
                    AwayDangerousAttacks = table.Column<short>(type: "smallint", nullable: false),
                    HomeAttacks = table.Column<short>(type: "smallint", nullable: false),
                    AwayAttacks = table.Column<short>(type: "smallint", nullable: false),
                    HomePenalties = table.Column<short>(type: "smallint", nullable: false),
                    AwayPenalties = table.Column<short>(type: "smallint", nullable: false),
                    HomePossession = table.Column<short>(type: "smallint", nullable: false),
                    AwayPossession = table.Column<short>(type: "smallint", nullable: false),
                    MatchTimeSeconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchData");
        }
    }
}
