using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class userquota072230 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userQuotas",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LiveInsightPerGame = table.Column<int>(type: "int", nullable: false),
                    LivePredictionPerGAme = table.Column<int>(type: "int", nullable: false),
                    MaxPredictions = table.Column<int>(type: "int", nullable: false),
                    AccumulatorGenerators = table.Column<int>(type: "int", nullable: false),
                    ShockDetectors = table.Column<int>(type: "int", nullable: false),
                    CleverLabelling = table.Column<int>(type: "int", nullable: false),
                    HistoryOfAccumulators = table.Column<int>(type: "int", nullable: false),
                    WiseProHedge = table.Column<int>(type: "int", nullable: false),
                    LeagueStatistics = table.Column<int>(type: "int", nullable: false),
                    WiseProIncluded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userQuotas", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_userQuotas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userQuotas");
        }
    }
}
