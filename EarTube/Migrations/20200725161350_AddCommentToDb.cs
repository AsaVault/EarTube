using Microsoft.EntityFrameworkCore.Migrations;

namespace EarTube.Migrations
{
    public partial class AddCommentToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentLikes",
                table: "Comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId1",
                table: "Comment",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_UserId1",
                table: "Comment",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_UserId1",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_UserId1",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CommentLikes",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Comment");
        }
    }
}
