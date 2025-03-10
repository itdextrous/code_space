using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class competitionLevel1217 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HostCountry",
                table: "Competitions",
                newName: "CountryId");

            migrationBuilder.AddColumn<int>(
                name: "LeagueLevel",
                table: "Competitions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueLevel",
                table: "Competitions");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Competitions",
                newName: "HostCountry");
        }
    }
}
