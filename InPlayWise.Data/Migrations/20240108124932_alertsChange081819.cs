using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class alertsChange081819 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertBeforeInMinutes",
                table: "MatchAlerts");

            migrationBuilder.RenameColumn(
                name: "MatchTime",
                table: "MatchAlerts",
                newName: "AlertTime");

            migrationBuilder.AddColumn<bool>(
                name: "EmailAlert",
                table: "MatchAlerts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationAlert",
                table: "MatchAlerts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAlert",
                table: "MatchAlerts");

            migrationBuilder.DropColumn(
                name: "NotificationAlert",
                table: "MatchAlerts");

            migrationBuilder.RenameColumn(
                name: "AlertTime",
                table: "MatchAlerts",
                newName: "MatchTime");

            migrationBuilder.AddColumn<short>(
                name: "AlertBeforeInMinutes",
                table: "MatchAlerts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
