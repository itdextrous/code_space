using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class advertisement081259 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userQuotas_AspNetUsers_UserId",
                table: "userQuotas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userQuotas",
                table: "userQuotas");

            migrationBuilder.RenameTable(
                name: "userQuotas",
                newName: "UserQuotas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuotas",
                table: "UserQuotas",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirmLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuotas_AspNetUsers_UserId",
                table: "UserQuotas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuotas_AspNetUsers_UserId",
                table: "UserQuotas");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuotas",
                table: "UserQuotas");

            migrationBuilder.RenameTable(
                name: "UserQuotas",
                newName: "userQuotas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userQuotas",
                table: "userQuotas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_userQuotas_AspNetUsers_UserId",
                table: "userQuotas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
