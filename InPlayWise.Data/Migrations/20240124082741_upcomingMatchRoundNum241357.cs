using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class upcomingMatchRoundNum241357 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupNum",
                table: "UpcomingMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoundNum",
                table: "UpcomingMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StageId",
                table: "UpcomingMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StageName",
                table: "UpcomingMatches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupNum",
                table: "UpcomingMatches");

            migrationBuilder.DropColumn(
                name: "RoundNum",
                table: "UpcomingMatches");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "UpcomingMatches");

            migrationBuilder.DropColumn(
                name: "StageName",
                table: "UpcomingMatches");
        }
    }
}
