using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class favTeamsAndComp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "FavouriteTeams");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "FavouriteTeams");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "FavouriteCompetitions");

            migrationBuilder.AddColumn<string>(
                name: "TeamId",
                table: "FavouriteTeams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FavouriteTeams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "FavouriteCompetitions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FavouriteCompetitions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteTeams_TeamId",
                table: "FavouriteTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteTeams_UserId",
                table: "FavouriteTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteCompetitions_CompetitionId",
                table: "FavouriteCompetitions",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteCompetitions_UserId",
                table: "FavouriteCompetitions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteCompetitions_AspNetUsers_UserId",
                table: "FavouriteCompetitions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteCompetitions_Competitions_CompetitionId",
                table: "FavouriteCompetitions",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteTeams_AspNetUsers_UserId",
                table: "FavouriteTeams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteTeams_Team_TeamId",
                table: "FavouriteTeams",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteCompetitions_AspNetUsers_UserId",
                table: "FavouriteCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteCompetitions_Competitions_CompetitionId",
                table: "FavouriteCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteTeams_AspNetUsers_UserId",
                table: "FavouriteTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteTeams_Team_TeamId",
                table: "FavouriteTeams");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteTeams_TeamId",
                table: "FavouriteTeams");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteTeams_UserId",
                table: "FavouriteTeams");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteCompetitions_CompetitionId",
                table: "FavouriteCompetitions");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteCompetitions_UserId",
                table: "FavouriteCompetitions");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "FavouriteTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FavouriteTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FavouriteCompetitions");

            migrationBuilder.AddColumn<string>(
                name: "CompetitionId",
                table: "FavouriteTeams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "FavouriteTeams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompetitionId",
                table: "FavouriteCompetitions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "FavouriteCompetitions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
