using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class venta_factura_origen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Origen",
                table: "VentaFacturas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OrigenCodigoArticulo",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OrigenImporte",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrigenNombreArticulo",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1155), new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1163) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(517), new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(823) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1179), new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1182) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origen",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "OrigenCodigoArticulo",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "OrigenImporte",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "OrigenNombreArticulo",
                table: "VentaFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(799), new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(808) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(180), new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(478) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(827), new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(829) });
        }
    }
}
