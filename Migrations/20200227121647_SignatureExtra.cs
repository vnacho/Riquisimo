using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class SignatureExtra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignatureAfter",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureBefore",
                table: "Congresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureAfter",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "SignatureBefore",
                table: "Congresses");
        }
    }
}
