using Microsoft.EntityFrameworkCore.Migrations;

namespace EarTube.Migrations
{
    public partial class AddingUserIdToSongDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Song",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Song_UserId",
                table: "Song",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_AspNetUsers_UserId",
                table: "Song",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_AspNetUsers_UserId",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Song_UserId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Song");
        }
    }
}
