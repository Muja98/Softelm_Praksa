using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AddedAtributesForTransactionAndJobApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "WorksOn",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnnouncementTitle",
                table: "JobApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FarmName",
                table: "JobApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AnnouncementTitle",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "FarmName",
                table: "JobApplications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "WorksOn",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
