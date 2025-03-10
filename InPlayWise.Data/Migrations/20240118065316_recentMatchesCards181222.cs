using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class recentMatchesCards181222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwayYellowRedCards",
                table: "RecentMatches",
                newName: "AwayYellowCards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwayYellowCards",
                table: "RecentMatches",
                newName: "AwayYellowRedCards");
        }
    }
}
