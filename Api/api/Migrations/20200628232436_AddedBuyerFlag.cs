using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AddedBuyerFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BuyerFlag",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerFlag",
                table: "Users");
        }
    }
}
