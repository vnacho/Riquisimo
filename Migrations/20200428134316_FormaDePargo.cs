using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class FormaDePargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FPag",
                table: "Registrations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FPag",
                table: "Expenses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FPag",
                table: "Accommodations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FPag",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "FPag",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "FPag",
                table: "Accommodations");
        }
    }
}
