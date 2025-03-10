using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class featureCounterIdRemove1825 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCounters",
                table: "FeatureCounters");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCounters_UserId",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FeatureCounters");

            migrationBuilder.DropColumn(
                name: "AccumulatorGenerators",
                table: "FeatureCounters");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FeatureCounters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCounters",
                table: "FeatureCounters",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "LiveInsightsPerGameCounters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiHitsRemaining = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveInsightsPerGameCounters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveInsightsPerGameCounters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCounters",
                table: "FeatureCounters");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FeatureCounters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FeatureCounters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AccumulatorGenerators",
                table: "FeatureCounters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCounters",
                table: "FeatureCounters",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCounters_UserId",
                table: "FeatureCounters",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
