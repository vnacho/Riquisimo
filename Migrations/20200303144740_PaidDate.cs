using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class PaidDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Accommodations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Accommodations");
        }
    }
}
