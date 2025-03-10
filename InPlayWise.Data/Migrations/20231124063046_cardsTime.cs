using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class cardsTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayCornerMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayRedMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayShotsMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayShotsOnTargetMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwaySubstitutionMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayYellowMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeCornerMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeRedMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeShotsMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeShotsOnTargetMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeSubstitutionMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeYellowMinutes",
                table: "LiveMatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
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
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropColumn(
                name: "AwayCornerMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayRedMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayShotsMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayShotsOnTargetMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwaySubstitutionMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "AwayYellowMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeCornerMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeRedMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeShotsMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeShotsOnTargetMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeSubstitutionMinutes",
                table: "LiveMatches");

            migrationBuilder.DropColumn(
                name: "HomeYellowMinutes",
                table: "LiveMatches");
        }
    }
}
