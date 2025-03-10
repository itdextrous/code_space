using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class PenaltiesRecord070042 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayPenaltiesRecord",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePenaltiesRecord",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayPenaltiesRecord",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePenaltiesRecord",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayPenaltiesRecord",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomePenaltiesRecord",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayPenaltiesRecord",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomePenaltiesRecord",
                table: "LiveMatches");
        }
    }
}
