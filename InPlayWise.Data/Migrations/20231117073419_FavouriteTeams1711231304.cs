using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class FavouriteTeams1711231304 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "TeamModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNational = table.Column<bool>(type: "bit", nullable: false),
                    CountryLogo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserTeamModel",
                columns: table => new
                {
                    FavouriteTeamsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTeamModel", x => new { x.FavouriteTeamsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTeamModel_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTeamModel_TeamModel_FavouriteTeamsId",
                        column: x => x.FavouriteTeamsId,
                        principalTable: "TeamModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTeamModel_UsersId",
                table: "ApplicationUserTeamModel",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTeamModel");

            migrationBuilder.DropTable(
                name: "TeamModel");

            migrationBuilder.AddColumn<string>(
                name: "ProfileUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileUserId",
                table: "AspNetUsers",
                column: "ProfileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileUserId",
                table: "AspNetUsers",
                column: "ProfileUserId",
                principalTable: "Profiles",
                principalColumn: "UserId");
        }
    }
}
