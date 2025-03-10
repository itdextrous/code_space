using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class numOfOpportunities191052 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "NumOfOpportunities",
                table: "LiveMatches",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfOpportunities",
                table: "LiveMatches");
        }
    }
}
