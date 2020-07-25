using Microsoft.EntityFrameworkCore.Migrations;

namespace EarTube.Migrations
{
    public partial class AddingANewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Song_SongId",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "SongLike",
                table: "Song",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SongId",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Song_SongId",
                table: "Comment",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Song_SongId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "SongLike",
                table: "Song");

            migrationBuilder.AlterColumn<int>(
                name: "SongId",
                table: "Comment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Song_SongId",
                table: "Comment",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
