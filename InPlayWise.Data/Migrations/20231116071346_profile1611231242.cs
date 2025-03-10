using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class profile1611231242 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_predictionData",
                table: "predictionData");

            migrationBuilder.RenameTable(
                name: "predictionData",
                newName: "PredictionData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredictionData",
                table: "PredictionData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PredictionData",
                table: "PredictionData");

            migrationBuilder.RenameTable(
                name: "PredictionData",
                newName: "predictionData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_predictionData",
                table: "predictionData",
                column: "Id");
        }
    }
}
