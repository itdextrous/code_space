using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class livematches07150635 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamId",
                table: "LiveMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamId",
                table: "LiveMatches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
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

            migrationBuilder.AddForeignKey(
                name: "FK_LiveMatches_Team_AwayTeamId",
                table: "LiveMatches",
                column: "AwayTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveMatches_Team_HomeTeamId",
                table: "LiveMatches",
                column: "HomeTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveMatches_Team_AwayTeamId",
                table: "LiveMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveMatches_Team_HomeTeamId",
                table: "LiveMatches");

            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_AwayTeamId",
                table: "LiveMatches");

            migrationBuilder.DropIndex(
                name: "IX_LiveMatches_HomeTeamId",
                table: "LiveMatches");

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamId",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamId",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
