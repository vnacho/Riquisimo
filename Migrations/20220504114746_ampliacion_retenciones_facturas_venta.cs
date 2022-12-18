using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ampliacion_retenciones_facturas_venta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsRetencionFiscal",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsRetencionNoFiscal",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModoRetencion",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3197), new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3205) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(2571), new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3223), new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3226) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsRetencionFiscal",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "EsRetencionNoFiscal",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "ModoRetencion",
                table: "VentaFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1506), new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1528) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(848), new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1166) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1547), new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1550) });
        }
    }
}
