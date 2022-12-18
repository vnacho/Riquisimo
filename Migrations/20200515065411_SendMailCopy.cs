using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class SendMailCopy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCongress",
                table: "Congresses",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendCopyTo",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SendCopyTo",
                table: "Accounts");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCongress",
                table: "Congresses",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8602), new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8613) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(7419), new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(7998) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8635), new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8638) });
        }
    }
}
