﻿// <auto-generated />
using System;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InPlayWise.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231130060918_membership1139")]
    partial class membership1139
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("InPlayWise.Common.DTO.PredictionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("AwayAttacks")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayCorners")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayDangerousAttacks")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayOwnGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayPenalties")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayPossession")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayRed")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayShotsOffTarget")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayShotsOnTarget")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayYellow")
                        .HasColumnType("smallint");

                    b.Property<short>("ExtraTime")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeAttacks")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeCorners")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeDangerousAttacks")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeOwnGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("HomePenalties")
                        .HasColumnType("smallint");

                    b.Property<short>("HomePossession")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeRed")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeShotsOffTarget")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeShotsOnTarget")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeYellow")
                        .HasColumnType("smallint");

                    b.Property<short>("Incident")
                        .HasColumnType("smallint");

                    b.Property<string>("MatchId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("MatchMinutes")
                        .HasColumnType("smallint");

                    b.Property<short>("MatchStatus")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("PredictionData");
                });

            modelBuilder.Entity("InPlayWise.Common.SportsEntities.LiveMatchModel", b =>
                {
                    b.Property<string>("MatchId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<short>("AwayAttacks")
                        .HasColumnType("smallint");

                    b.Property<string>("AwayCornerMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("AwayCorners")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayDangerousAttacks")
                        .HasColumnType("smallint");

                    b.Property<string>("AwayGoalMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("AwayGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayOwnGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayPenalties")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayPossession")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayRed")
                        .HasColumnType("smallint");

                    b.Property<string>("AwayRedMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayShotsMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("AwayShotsOffTarget")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayShotsOnTarget")
                        .HasColumnType("smallint");

                    b.Property<string>("AwayShotsOnTargetMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwaySubstitutionMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayTeamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayTeamLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayTeamName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayTeamShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("AwayYellow")
                        .HasColumnType("smallint");

                    b.Property<string>("AwayYellowMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompetitionLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompetitionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompetitionShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("HomeAttacks")
                        .HasColumnType("smallint");

                    b.Property<string>("HomeCornerMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("HomeCorners")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeDangerousAttacks")
                        .HasColumnType("smallint");

                    b.Property<string>("HomeGoalMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("HomeGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeOwnGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("HomePenalties")
                        .HasColumnType("smallint");

                    b.Property<short>("HomePossession")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeRed")
                        .HasColumnType("smallint");

                    b.Property<string>("HomeRedMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeShotsMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("HomeShotsOffTarget")
                        .HasColumnType("smallint");

                    b.Property<short>("HomeShotsOnTarget")
                        .HasColumnType("smallint");

                    b.Property<string>("HomeShotsOnTargetMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeSubstitutionMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("HomeYellow")
                        .HasColumnType("smallint");

                    b.Property<string>("HomeYellowMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("MatchMinutes")
                        .HasColumnType("smallint");

                    b.Property<long>("MatchStartTime")
                        .HasColumnType("bigint");

                    b.Property<short>("MatchStatus")
                        .HasColumnType("smallint");

                    b.Property<string>("SeasonId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MatchId");

                    b.ToTable("LiveMatches");
                });

            modelBuilder.Entity("InPlayWise.Common.SportsEntities.RecentMatchModel", b =>
                {
                    b.Property<string>("MatchId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AwayAttacks")
                        .HasColumnType("int");

                    b.Property<string>("AwayCornerMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AwayCorners")
                        .HasColumnType("int");

                    b.Property<int>("AwayDangerousAttacks")
                        .HasColumnType("int");

                    b.Property<string>("AwayGoalMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AwayGoals")
                        .HasColumnType("int");

                    b.Property<short>("AwayOwnGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("AwayPenalties")
                        .HasColumnType("smallint");

                    b.Property<int>("AwayPossession")
                        .HasColumnType("int");

                    b.Property<int>("AwayRedCards")
                        .HasColumnType("int");

                    b.Property<string>("AwayRedMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayShotsMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AwayShotsOffTarget")
                        .HasColumnType("int");

                    b.Property<int>("AwayShotsOnTarget")
                        .HasColumnType("int");

                    b.Property<string>("AwayShotsOnTargetMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwaySubstitutionMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayTeamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayTeamName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AwayYellowMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AwayYellowRedCards")
                        .HasColumnType("int");

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompetitionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("GameDrawn")
                        .HasColumnType("bit");

                    b.Property<int>("HomeAttacks")
                        .HasColumnType("int");

                    b.Property<string>("HomeCornerMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeCorners")
                        .HasColumnType("int");

                    b.Property<int>("HomeDangerousAttacks")
                        .HasColumnType("int");

                    b.Property<string>("HomeGoalMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeGoals")
                        .HasColumnType("int");

                    b.Property<short>("HomeOwnGoals")
                        .HasColumnType("smallint");

                    b.Property<short>("HomePenalties")
                        .HasColumnType("smallint");

                    b.Property<int>("HomePossession")
                        .HasColumnType("int");

                    b.Property<int>("HomeRedCards")
                        .HasColumnType("int");

                    b.Property<string>("HomeRedMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeShotsMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeShotsOffTarget")
                        .HasColumnType("int");

                    b.Property<int>("HomeShotsOnTarget")
                        .HasColumnType("int");

                    b.Property<string>("HomeShotsOnTargetMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeSubstitutionMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HomeWin")
                        .HasColumnType("bit");

                    b.Property<int>("HomeYellowCards")
                        .HasColumnType("int");

                    b.Property<string>("HomeYellowMinutes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MatchStartTime")
                        .HasColumnType("bigint");

                    b.Property<string>("SeasonId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MatchId");

                    b.ToTable("RecentMatches");
                });

            modelBuilder.Entity("InPlayWise.Common.SportsEntities.TeamModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsNational")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubscriptionExpires")
                        .HasColumnType("datetime2");

                    b.Property<bool>("ThemeIsDark")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.FavouriteCompetitions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FavouriteCompetitions");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.FavouriteTeams", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FavouriteTeams");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.MatchStatus", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MatchStates");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.MembershipEntities.Price", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IntervalInDays")
                        .HasColumnType("int");

                    b.Property<int>("PriceInCents")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StripeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.MembershipEntities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.ResetPasswordModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ResetPassword");
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.UserProfile", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ThemeDark")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("InPlayWiseData.Entities.EventModel", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventGround")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.MembershipEntities.Price", b =>
                {
                    b.HasOne("InPlayWise.Data.Entities.MembershipEntities.Product", "Product")
                        .WithMany("Price")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InPlayWise.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InPlayWise.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InPlayWise.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("InPlayWise.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InPlayWise.Data.Entities.MembershipEntities.Product", b =>
                {
                    b.Navigation("Price");
                });
#pragma warning restore 612, 618
        }
    }
}
