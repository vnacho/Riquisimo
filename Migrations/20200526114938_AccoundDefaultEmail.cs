using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AccoundDefaultEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomingMailPort",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IncomingMailServer",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailPassword",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailUser",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutgoingMailPort",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingMailServer",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureAfter",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureBefore",
                table: "Accounts",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6845), new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6859) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(5563), new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6140) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6882), new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6885) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomingMailPort",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IncomingMailServer",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MailPassword",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MailUser",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OutgoingMailPort",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OutgoingMailServer",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SignatureAfter",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SignatureBefore",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 15, 8, 54, 10, 914, DateTimeKind.Local).AddTicks(9068), new DateTime(2020, 5, 15, 8, 54, 10, 914, DateTimeKind.Local).AddTicks(9079) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 15, 8, 54, 10, 914, DateTimeKind.Local).AddTicks(7937), new DateTime(2020, 5, 15, 8, 54, 10, 914, DateTimeKind.Local).AddTicks(8490) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 15, 8, 54, 10, 914, DateTimeKind.Local).AddTicks(9101), new DateTime(2020, 5, 15, 8, 54, 10, 914, DateTimeKind.Local).AddTicks(9104) });
        }
    }
}
