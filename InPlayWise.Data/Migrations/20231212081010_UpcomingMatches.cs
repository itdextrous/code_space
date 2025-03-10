using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class UpcomingMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpcomingMatches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HomeTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpcomingMatches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpcomingMatches");
        }
    }
}
