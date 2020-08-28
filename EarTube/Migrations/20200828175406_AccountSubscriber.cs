using Microsoft.EntityFrameworkCore.Migrations;

namespace EarTube.Migrations
{
    public partial class AccountSubscriber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSubscriber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountUserId = table.Column<string>(nullable: true),
                    SubscribeUserId = table.Column<string>(nullable: true),
                    SubscribeUserEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSubscriber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountUnsubscriber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountUserId = table.Column<string>(nullable: true),
                    UnSubscribeUserId = table.Column<string>(nullable: true),
                    UnSubscribeUserEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountUnsubscriber", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSubscriber");

            migrationBuilder.DropTable(
                name: "AccountUnsubscriber");
        }
    }
}
