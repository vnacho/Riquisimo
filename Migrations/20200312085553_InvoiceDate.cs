using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class InvoiceDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "Accommodations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "Accommodations");
        }
    }
}
