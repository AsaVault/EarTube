using Microsoft.EntityFrameworkCore.Migrations;

namespace EarTube.Migrations
{
    public partial class UserNamesToAccountSubscriberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriberFirstName",
                table: "AccountSubscriber",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriberLastName",
                table: "AccountSubscriber",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriberFirstName",
                table: "AccountSubscriber");

            migrationBuilder.DropColumn(
                name: "SubscriberLastName",
                table: "AccountSubscriber");
        }
    }
}
