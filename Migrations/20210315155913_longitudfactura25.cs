using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class longitudfactura25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroFactura",
                table: "CompraFacturas",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 15, 16, 59, 12, 448, DateTimeKind.Local).AddTicks(6031), new DateTime(2021, 3, 15, 16, 59, 12, 448, DateTimeKind.Local).AddTicks(6040) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 15, 16, 59, 12, 448, DateTimeKind.Local).AddTicks(4900), new DateTime(2021, 3, 15, 16, 59, 12, 448, DateTimeKind.Local).AddTicks(5449) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 15, 16, 59, 12, 448, DateTimeKind.Local).AddTicks(6061), new DateTime(2021, 3, 15, 16, 59, 12, 448, DateTimeKind.Local).AddTicks(6064) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroFactura",
                table: "CompraFacturas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 24);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8898), new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8911) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(7715), new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8288) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8934), new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8937) });
        }
    }
}
