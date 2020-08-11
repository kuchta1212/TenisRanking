using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TenisRanking.Data.Migrations
{
    public partial class Iit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPlayedMatch",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PlayerOneName = table.Column<string>(nullable: true),
                    PlayerTwoName = table.Column<string>(nullable: true),
                    AmountOfGamesPlayerOne = table.Column<int>(nullable: false),
                    AmountOfGamesPlayerTwo = table.Column<int>(nullable: false),
                    DateOfGame = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastPlayedMatch",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "AspNetUsers");
        }
    }
}
