using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ExtraCongressFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailPort",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "MailServer",
                table: "Congresses");

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomingMailPort",
                table: "Congresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IncomingMailServer",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutgoingMailPort",
                table: "Congresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingMailServer",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SageAccount",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Congresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "IncomingMailPort",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "IncomingMailServer",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "OutgoingMailPort",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "OutgoingMailServer",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "SageAccount",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Congresses");

            migrationBuilder.AddColumn<int>(
                name: "MailPort",
                table: "Congresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MailServer",
                table: "Congresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
