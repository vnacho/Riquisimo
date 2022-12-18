using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ProductDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VAT",
                table: "Registrations",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VAT",
                table: "Products",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VAT",
                table: "Expenses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VAT",
                table: "Accommodations",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Accommodations");
        }
    }
}
