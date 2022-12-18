using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class importedescuento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ImporteDescuento",
                table: "VentaPedidoLineas",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ImporteDescuento",
                table: "VentaFacturaLineas",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ImporteDescuento",
                table: "VentaAlbaranLineas",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 10, 50, 44, 9, DateTimeKind.Local).AddTicks(6245), new DateTime(2022, 1, 25, 10, 50, 44, 9, DateTimeKind.Local).AddTicks(6255) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 10, 50, 44, 9, DateTimeKind.Local).AddTicks(5620), new DateTime(2022, 1, 25, 10, 50, 44, 9, DateTimeKind.Local).AddTicks(5925) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 10, 50, 44, 9, DateTimeKind.Local).AddTicks(6275), new DateTime(2022, 1, 25, 10, 50, 44, 9, DateTimeKind.Local).AddTicks(6278) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImporteDescuento",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "ImporteDescuento",
                table: "VentaFacturaLineas");

            migrationBuilder.DropColumn(
                name: "ImporteDescuento",
                table: "VentaAlbaranLineas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 9, 58, 49, 810, DateTimeKind.Local).AddTicks(1040), new DateTime(2022, 1, 25, 9, 58, 49, 810, DateTimeKind.Local).AddTicks(1050) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 9, 58, 49, 810, DateTimeKind.Local).AddTicks(393), new DateTime(2022, 1, 25, 9, 58, 49, 810, DateTimeKind.Local).AddTicks(707) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 9, 58, 49, 810, DateTimeKind.Local).AddTicks(1069), new DateTime(2022, 1, 25, 9, 58, 49, 810, DateTimeKind.Local).AddTicks(1072) });
        }
    }
}
