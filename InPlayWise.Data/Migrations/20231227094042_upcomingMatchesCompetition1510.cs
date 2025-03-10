using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class upcomingMatchesCompetition1510 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "UpcomingMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingMatches_CompetitionId",
                table: "UpcomingMatches",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UpcomingMatches_Competitions_CompetitionId",
                table: "UpcomingMatches",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpcomingMatches_Competitions_CompetitionId",
                table: "UpcomingMatches");

            migrationBuilder.DropIndex(
                name: "IX_UpcomingMatches_CompetitionId",
                table: "UpcomingMatches");

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "UpcomingMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
