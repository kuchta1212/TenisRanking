using Microsoft.EntityFrameworkCore.Migrations;

namespace TenisRanking.Data.Migrations
{
    public partial class playerNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "AspNetUsers");
        }
    }
}
