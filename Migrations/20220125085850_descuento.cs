using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class descuento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatoImpresion",
                table: "VentaPedidos");

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "VentaPedidoLineas",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "VentaFacturaLineas",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "VentaAlbaranLineas",
                nullable: false,
                defaultValue: 0m);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "VentaFacturaLineas");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "VentaAlbaranLineas");

            migrationBuilder.AddColumn<int>(
                name: "FormatoImpresion",
                table: "VentaPedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7699), new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7707) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7024), new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7330) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7724), new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7727) });
        }
    }
}
