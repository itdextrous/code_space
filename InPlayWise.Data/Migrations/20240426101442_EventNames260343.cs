using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class EventNames260343 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayRedNames",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwaySubstituteNames",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayYellowNames",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeRedNames",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeSubstituteNames",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeYellowNames",
                table: "RecentMatches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayRedNames",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwaySubstituteNames",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "AwayYellowNames",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeRedNames",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeSubstituteNames",
                table: "RecentMatches");

            migrationBuilder.DropColumn(
                name: "HomeYellowNames",
                table: "RecentMatches");
        }
    }
}
