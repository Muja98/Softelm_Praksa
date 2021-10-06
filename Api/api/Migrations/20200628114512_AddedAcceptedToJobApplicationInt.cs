using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AddedAcceptedToJobApplicationInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TypesOfAnimal");

            migrationBuilder.AlterColumn<int>(
                name: "Accepted",
                table: "JobApplications",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TypesOfAnimal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "Accepted",
                table: "JobApplications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
