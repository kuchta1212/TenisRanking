using Microsoft.EntityFrameworkCore.Migrations;

namespace TenisRanking.Data.Migrations
{
    public partial class FixingMatchResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "SecondSetDefender",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ThirdSetChellanger",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ThirdSetDefender",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "MatchResult",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MatchResult",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Winner = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchSet",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Challanger = table.Column<int>(nullable: false),
                    Deffender = table.Column<int>(nullable: false),
                    ChallengerTieBreak = table.Column<int>(nullable: false),
                    DeffenderTieBreak = table.Column<int>(nullable: false),
                    MatchResultId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchSet_MatchResult_MatchResultId",
                        column: x => x.MatchResultId,
                        principalTable: "MatchResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchResult",
                table: "Matches",
                column: "MatchResult");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSet_MatchResultId",
                table: "MatchSet",
                column: "MatchResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_MatchResult_MatchResult",
                table: "Matches",
                column: "MatchResult",
                principalTable: "MatchResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchResult_MatchResult",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "MatchSet");

            migrationBuilder.DropTable(
                name: "MatchResult");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchResult",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchResult",
                table: "Matches");

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

            migrationBuilder.AddColumn<int>(
                name: "SecondSetDefender",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThirdSetChellanger",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThirdSetDefender",
                table: "Matches",
                nullable: false,
                defaultValue: 0);
        }
    }
}
