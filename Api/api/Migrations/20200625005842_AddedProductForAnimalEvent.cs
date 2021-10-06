using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AddedProductForAnimalEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AnimalEvents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalEvents_ProductId",
                table: "AnimalEvents",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalEvents_Products_ProductId",
                table: "AnimalEvents",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalEvents_Products_ProductId",
                table: "AnimalEvents");

            migrationBuilder.DropIndex(
                name: "IX_AnimalEvents_ProductId",
                table: "AnimalEvents");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AnimalEvents");
        }
    }
}
