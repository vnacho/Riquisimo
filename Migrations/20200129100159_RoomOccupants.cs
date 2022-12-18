using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class RoomOccupants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occupants",
                table: "Accommodations");

            migrationBuilder.AddColumn<int>(
                name: "Occupants",
                table: "RoomTypes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occupants",
                table: "RoomTypes");

            migrationBuilder.AddColumn<int>(
                name: "Occupants",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
