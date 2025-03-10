using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class teamRankUpcoming092350 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleverLabelling",
                table: "UserQuotas");

            migrationBuilder.DropColumn(
                name: "LiveInsightPerGame",
                table: "UserQuotas");

            migrationBuilder.RenameColumn(
                name: "MaxPredictions",
                table: "UserQuotas",
                newName: "TotalPrediction");

            migrationBuilder.RenameColumn(
                name: "LivePredictionPerGAme",
                table: "UserQuotas",
                newName: "CleverLabels");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamRank",
                table: "UpcomingMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamRank",
                table: "UpcomingMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamRank",
                table: "UpcomingMatches");

            migrationBuilder.DropColumn(
                name: "HomeTeamRank",
                table: "UpcomingMatches");

            migrationBuilder.RenameColumn(
                name: "TotalPrediction",
                table: "UserQuotas",
                newName: "MaxPredictions");

            migrationBuilder.RenameColumn(
                name: "CleverLabels",
                table: "UserQuotas",
                newName: "LivePredictionPerGAme");

            migrationBuilder.AddColumn<int>(
                name: "CleverLabelling",
                table: "UserQuotas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LiveInsightPerGame",
                table: "UserQuotas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
