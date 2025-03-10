using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class UnusedTables032351 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuotas_AspNetUsers_UserId",
                table: "UserQuotas");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuotas",
                table: "UserQuotas");

            migrationBuilder.RenameTable(
                name: "UserQuotas",
                newName: "UserQuota");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuota",
                table: "UserQuota",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuota_AspNetUsers_UserId",
                table: "UserQuota",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuota_AspNetUsers_UserId",
                table: "UserQuota");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuota",
                table: "UserQuota");

            migrationBuilder.RenameTable(
                name: "UserQuota",
                newName: "UserQuotas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuotas",
                table: "UserQuotas",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNational = table.Column<bool>(type: "bit", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuotas_AspNetUsers_UserId",
                table: "UserQuotas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
