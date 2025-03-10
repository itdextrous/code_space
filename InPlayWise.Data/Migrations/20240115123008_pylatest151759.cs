using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class pylatest151759 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_LiveMatches_MatchId",
                table: "Opportunities");

            migrationBuilder.AlterColumn<float>(
                name: "Prediction",
                table: "Opportunities",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_LiveMatches_MatchId",
                table: "Opportunities",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_LiveMatches_MatchId",
                table: "Opportunities");

            migrationBuilder.AlterColumn<int>(
                name: "Prediction",
                table: "Opportunities",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_LiveMatches_MatchId",
                table: "Opportunities",
                column: "MatchId",
                principalTable: "LiveMatches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
