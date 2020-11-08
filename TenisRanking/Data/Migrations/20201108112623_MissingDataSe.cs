using Microsoft.EntityFrameworkCore.Migrations;

namespace TenisRanking.Data.Migrations
{
    public partial class MissingDataSe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchResult_MatchResult",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchSet_MatchResult_MatchResultId",
                table: "MatchSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchSet",
                table: "MatchSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchResult",
                table: "MatchResult");

            migrationBuilder.RenameTable(
                name: "MatchSet",
                newName: "Sets");

            migrationBuilder.RenameTable(
                name: "MatchResult",
                newName: "Results");

            migrationBuilder.RenameIndex(
                name: "IX_MatchSet_MatchResultId",
                table: "Sets",
                newName: "IX_Sets_MatchResultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sets",
                table: "Sets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Results_MatchResult",
                table: "Matches",
                column: "MatchResult",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Results_MatchResultId",
                table: "Sets",
                column: "MatchResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Results_MatchResult",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Results_MatchResultId",
                table: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sets",
                table: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.RenameTable(
                name: "Sets",
                newName: "MatchSet");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "MatchResult");

            migrationBuilder.RenameIndex(
                name: "IX_Sets_MatchResultId",
                table: "MatchSet",
                newName: "IX_MatchSet_MatchResultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchSet",
                table: "MatchSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchResult",
                table: "MatchResult",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_MatchResult_MatchResult",
                table: "Matches",
                column: "MatchResult",
                principalTable: "MatchResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchSet_MatchResult_MatchResultId",
                table: "MatchSet",
                column: "MatchResultId",
                principalTable: "MatchResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
