using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class upcomingTeam1728 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamId",
                table: "UpcomingMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamId",
                table: "UpcomingMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingMatches_AwayTeamId",
                table: "UpcomingMatches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingMatches_HomeTeamId",
                table: "UpcomingMatches",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_UpcomingMatches_Team_AwayTeamId",
                table: "UpcomingMatches",
                column: "AwayTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UpcomingMatches_Team_HomeTeamId",
                table: "UpcomingMatches",
                column: "HomeTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpcomingMatches_Team_AwayTeamId",
                table: "UpcomingMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_UpcomingMatches_Team_HomeTeamId",
                table: "UpcomingMatches");

            migrationBuilder.DropIndex(
                name: "IX_UpcomingMatches_AwayTeamId",
                table: "UpcomingMatches");

            migrationBuilder.DropIndex(
                name: "IX_UpcomingMatches_HomeTeamId",
                table: "UpcomingMatches");

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamId",
                table: "UpcomingMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamId",
                table: "UpcomingMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
