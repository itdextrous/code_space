using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class AddOddPlaced113901 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OppPlaced",
                table: "Accumulaters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OppResult",
                table: "Accumulaters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ResultTime",
                table: "Accumulaters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OppPlaced",
                table: "Accumulaters");

            migrationBuilder.DropColumn(
                name: "OppResult",
                table: "Accumulaters");

            migrationBuilder.DropColumn(
                name: "ResultTime",
                table: "Accumulaters");
        }
    }
}
