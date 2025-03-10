using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventGround = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "LiveMatches",
                columns: table => new
                {
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatchStatus = table.Column<short>(type: "smallint", nullable: false),
                    MatchMinutes = table.Column<short>(type: "smallint", nullable: false),
                    HomeGoals = table.Column<short>(type: "smallint", nullable: false),
                    AwayGoals = table.Column<short>(type: "smallint", nullable: false),
                    HomeRed = table.Column<short>(type: "smallint", nullable: false),
                    AwayRed = table.Column<short>(type: "smallint", nullable: false),
                    HomeYellow = table.Column<short>(type: "smallint", nullable: false),
                    AwayYellow = table.Column<short>(type: "smallint", nullable: false),
                    HomeCorners = table.Column<short>(type: "smallint", nullable: false),
                    AwayCorners = table.Column<short>(type: "smallint", nullable: false),
                    HomeShotsOnTarget = table.Column<short>(type: "smallint", nullable: false),
                    HomeShotsOffTarget = table.Column<short>(type: "smallint", nullable: false),
                    AwayShotsOnTarget = table.Column<short>(type: "smallint", nullable: false),
                    AwayShotsOffTarget = table.Column<short>(type: "smallint", nullable: false),
                    HomeDangerousAttacks = table.Column<short>(type: "smallint", nullable: false),
                    AwayDangerousAttacks = table.Column<short>(type: "smallint", nullable: false),
                    HomeAttacks = table.Column<short>(type: "smallint", nullable: false),
                    AwayAttacks = table.Column<short>(type: "smallint", nullable: false),
                    HomePenalties = table.Column<short>(type: "smallint", nullable: false),
                    AwayPenalties = table.Column<short>(type: "smallint", nullable: false),
                    HomePossession = table.Column<short>(type: "smallint", nullable: false),
                    AwayPossession = table.Column<short>(type: "smallint", nullable: false),
                    HomeOwnGoals = table.Column<short>(type: "smallint", nullable: false),
                    AwayOwnGoals = table.Column<short>(type: "smallint", nullable: false),
                    HomeGoalMinutes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayGoalMinutes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchStartTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveMatches", x => x.MatchId);
                });

            migrationBuilder.CreateTable(
                name: "MatchStates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "predictionData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchStatus = table.Column<short>(type: "smallint", nullable: false),
                    MatchMinutes = table.Column<short>(type: "smallint", nullable: false),
                    ExtraTime = table.Column<short>(type: "smallint", nullable: false),
                    HomeGoals = table.Column<short>(type: "smallint", nullable: false),
                    AwayGoals = table.Column<short>(type: "smallint", nullable: false),
                    HomeRed = table.Column<short>(type: "smallint", nullable: false),
                    AwayRed = table.Column<short>(type: "smallint", nullable: false),
                    HomeYellow = table.Column<short>(type: "smallint", nullable: false),
                    AwayYellow = table.Column<short>(type: "smallint", nullable: false),
                    HomeCorners = table.Column<short>(type: "smallint", nullable: false),
                    AwayCorners = table.Column<short>(type: "smallint", nullable: false),
                    HomeShotsOnTarget = table.Column<short>(type: "smallint", nullable: false),
                    HomeShotsOffTarget = table.Column<short>(type: "smallint", nullable: false),
                    AwayShotsOnTarget = table.Column<short>(type: "smallint", nullable: false),
                    AwayShotsOffTarget = table.Column<short>(type: "smallint", nullable: false),
                    Incident = table.Column<short>(type: "smallint", nullable: false),
                    HomeDangerousAttacks = table.Column<short>(type: "smallint", nullable: false),
                    AwayDangerousAttacks = table.Column<short>(type: "smallint", nullable: false),
                    HomeAttacks = table.Column<short>(type: "smallint", nullable: false),
                    AwayAttacks = table.Column<short>(type: "smallint", nullable: false),
                    HomePenalties = table.Column<short>(type: "smallint", nullable: false),
                    AwayPenalties = table.Column<short>(type: "smallint", nullable: false),
                    HomePossession = table.Column<short>(type: "smallint", nullable: false),
                    AwayPossession = table.Column<short>(type: "smallint", nullable: false),
                    HomeOwnGoals = table.Column<short>(type: "smallint", nullable: false),
                    AwayOwnGoals = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_predictionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecentMatches",
                columns: table => new
                {
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HomeTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeWin = table.Column<bool>(type: "bit", nullable: false),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    GameDrawn = table.Column<bool>(type: "bit", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    HomeRedCards = table.Column<int>(type: "int", nullable: false),
                    HomeYellowCards = table.Column<int>(type: "int", nullable: false),
                    AwayRedCards = table.Column<int>(type: "int", nullable: false),
                    AwayYellowRedCards = table.Column<int>(type: "int", nullable: false),
                    MatchStartTime = table.Column<long>(type: "bigint", nullable: false),
                    HomeGoalMinutes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayGoalMinutes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomeShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    HomeAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayAttacks = table.Column<int>(type: "int", nullable: false),
                    HomeDangerousAttacks = table.Column<int>(type: "int", nullable: false),
                    AwayDangerousAttacks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentMatches", x => x.MatchId);
                });

            migrationBuilder.CreateTable(
                name: "ResetPassword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPassword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "LiveMatches");

            migrationBuilder.DropTable(
                name: "MatchStates");

            migrationBuilder.DropTable(
                name: "predictionData");

            migrationBuilder.DropTable(
                name: "RecentMatches");

            migrationBuilder.DropTable(
                name: "ResetPassword");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
