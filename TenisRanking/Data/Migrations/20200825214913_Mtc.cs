using Microsoft.EntityFrameworkCore.Migrations;

namespace TenisRanking.Data.Migrations
{
    public partial class Mtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerTwoName",
                table: "Matches",
                newName: "Defender");

            migrationBuilder.RenameColumn(
                name: "PlayerOneName",
                table: "Matches",
                newName: "Chellanger");

            migrationBuilder.RenameColumn(
                name: "AmountOfGamesPlayerTwo",
                table: "Matches",
                newName: "ThirdSetDefender");

            migrationBuilder.RenameColumn(
                name: "AmountOfGamesPlayerOne",
                table: "Matches",
                newName: "SecondSetDefender");

            migrationBuilder.AddColumn<int>(
                name: "FirstSetChellanger",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstSetDefender",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondSetChellanger",
                table: "Matches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstSetChellanger",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FirstSetDefender",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondSetChellanger",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "ThirdSetDefender",
                table: "Matches",
                newName: "AmountOfGamesPlayerTwo");

            migrationBuilder.RenameColumn(
                name: "SecondSetDefender",
                table: "Matches",
                newName: "AmountOfGamesPlayerOne");

            migrationBuilder.RenameColumn(
                name: "Defender",
                table: "Matches",
                newName: "PlayerTwoName");

            migrationBuilder.RenameColumn(
                name: "Chellanger",
                table: "Matches",
                newName: "PlayerOneName");
        }
    }
}
