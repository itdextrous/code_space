using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class UpdatingSubscriptionTable051037 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentSubscription",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDowngrade",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpgrade",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreviousSubscription",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSubscription",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsDowngrade",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsUpgrade",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PreviousSubscription",
                table: "Subscriptions");
        }
    }
}
