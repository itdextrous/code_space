using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class competition07150648 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_AwayTeamId",
                table: "LiveMatches");

            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_HomeTeamId",
                table: "LiveMatches");

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "LiveMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveMatches_AwayTeamId",
                table: "LiveMatches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveMatches_CompetitionId",
                table: "LiveMatches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveMatches_HomeTeamId",
                table: "LiveMatches",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveMatches_Competitions_CompetitionId",
                table: "LiveMatches",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveMatches_Competitions_CompetitionId",
                table: "LiveMatches");

            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_AwayTeamId",
                table: "LiveMatches");

            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_CompetitionId",
                table: "LiveMatches");

            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_HomeTeamId",
                table: "LiveMatches");

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveMatches_AwayTeamId",
                table: "LiveMatches",
                column: "AwayTeamId",
                unique: true,
                filter: "[AwayTeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LiveMatches_HomeTeamId",
                table: "LiveMatches",
                column: "HomeTeamId",
                unique: true,
                filter: "[HomeTeamId] IS NOT NULL");
        }
    }
}
