using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class quitarrequeridos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Surnames",
                table: "Registrant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NIF",
                table: "Registrant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Registrant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 16, 47, 12, 474, DateTimeKind.Local).AddTicks(8676), new DateTime(2021, 10, 4, 16, 47, 12, 474, DateTimeKind.Local).AddTicks(8686) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 16, 47, 12, 474, DateTimeKind.Local).AddTicks(7820), new DateTime(2021, 10, 4, 16, 47, 12, 474, DateTimeKind.Local).AddTicks(8216) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 16, 47, 12, 474, DateTimeKind.Local).AddTicks(8708), new DateTime(2021, 10, 4, 16, 47, 12, 474, DateTimeKind.Local).AddTicks(8712) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Surnames",
                table: "Registrant",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NIF",
                table: "Registrant",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Registrant",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8173), new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8186) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(7173), new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(7619) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8210), new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8213) });
        }
    }
}
