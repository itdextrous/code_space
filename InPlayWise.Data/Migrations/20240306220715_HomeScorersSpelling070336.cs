using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class HomeScorersSpelling070336 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwayScoreres",
                table: "LiveMatches",
                newName: "AwayScorers");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwayScorers",
                table: "LiveMatches",
                newName: "AwayScoreres");
        }
    }
}
