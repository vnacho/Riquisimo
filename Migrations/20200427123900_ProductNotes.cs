using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ProductNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductNotes",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNotes",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNotes",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNotes",
                table: "Accommodations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductNotes",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "ProductNotes",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductNotes",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ProductNotes",
                table: "Accommodations");
        }
    }
}
