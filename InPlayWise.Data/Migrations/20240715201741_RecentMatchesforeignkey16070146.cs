using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class RecentMatchesforeignkey16070146 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamId",
                table: "RecentMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "RecentMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamId",
                table: "RecentMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecentMatches_AwayTeamId",
                table: "RecentMatches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentMatches_CompetitionId",
                table: "RecentMatches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentMatches_HomeTeamId",
                table: "RecentMatches",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecentMatches_Competitions_CompetitionId",
                table: "RecentMatches",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentMatches_Team_AwayTeamId",
                table: "RecentMatches",
                column: "AwayTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentMatches_Team_HomeTeamId",
                table: "RecentMatches",
                column: "HomeTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecentMatches_Competitions_CompetitionId",
                table: "RecentMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_RecentMatches_Team_AwayTeamId",
                table: "RecentMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_RecentMatches_Team_HomeTeamId",
                table: "RecentMatches");

            migrationBuilder.DropIndex(
                name: "IX_RecentMatches_AwayTeamId",
                table: "RecentMatches");

            migrationBuilder.DropIndex(
                name: "IX_RecentMatches_CompetitionId",
                table: "RecentMatches");

            migrationBuilder.DropIndex(
                name: "IX_RecentMatches_HomeTeamId",
                table: "RecentMatches");

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamId",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamId",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
