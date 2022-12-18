using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class regsitrofacturacompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Registro",
                table: "CompraFacturas",
                maxLength: 6,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3930), new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3939) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3316), new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3612) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3956), new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3959) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Registro",
                table: "CompraFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(415), new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(427) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 11, 19, 11, 55, 58, 795, DateTimeKind.Local).AddTicks(8973), new DateTime(2021, 11, 19, 11, 55, 58, 795, DateTimeKind.Local).AddTicks(9838) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(450), new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(454) });
        }
    }
}
