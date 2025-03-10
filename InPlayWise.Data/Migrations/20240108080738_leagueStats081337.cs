using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class leagueStats081337 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueStats",
                columns: table => new
                {
                    CompetitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OverOnePointFive = table.Column<int>(type: "int", nullable: false),
                    OverTwoPointFive = table.Column<int>(type: "int", nullable: false),
                    BTTS = table.Column<int>(type: "int", nullable: false),
                    GoalsPerMatch = table.Column<float>(type: "real", nullable: false),
                    CardsPerMatch = table.Column<float>(type: "real", nullable: false),
                    CornersPerMatch = table.Column<float>(type: "real", nullable: false),
                    MatchesThisSeason = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueStats", x => x.CompetitionId);
                    table.ForeignKey(
                        name: "FK_LeagueStats_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueStats");
        }
    }
}
