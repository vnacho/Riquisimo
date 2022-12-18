using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class iva_compras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseImponible",
                table: "CompraPedidos",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoIVA",
                table: "CompraPedidoLineas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IVA_Porcentaje",
                table: "CompraPedidoLineas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoIVA",
                table: "CompraAlbaranLineas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IVA_Porcentaje",
                table: "CompraAlbaranLineas",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseImponible",
                table: "CompraAlbaranes",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 30, 9, 35, 13, 797, DateTimeKind.Local).AddTicks(5381), new DateTime(2022, 3, 30, 9, 35, 13, 797, DateTimeKind.Local).AddTicks(5398) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 30, 9, 35, 13, 797, DateTimeKind.Local).AddTicks(4402), new DateTime(2022, 3, 30, 9, 35, 13, 797, DateTimeKind.Local).AddTicks(4819) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 30, 9, 35, 13, 797, DateTimeKind.Local).AddTicks(5428), new DateTime(2022, 3, 30, 9, 35, 13, 797, DateTimeKind.Local).AddTicks(5432) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseImponible",
                table: "CompraPedidos");

            migrationBuilder.DropColumn(
                name: "CodigoTipoIVA",
                table: "CompraPedidoLineas");

            migrationBuilder.DropColumn(
                name: "IVA_Porcentaje",
                table: "CompraPedidoLineas");

            migrationBuilder.DropColumn(
                name: "CodigoTipoIVA",
                table: "CompraAlbaranLineas");

            migrationBuilder.DropColumn(
                name: "IVA_Porcentaje",
                table: "CompraAlbaranLineas");

            migrationBuilder.DropColumn(
                name: "BaseImponible",
                table: "CompraAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4960), new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4970) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4303), new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4618) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4987), new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4990) });
        }
    }
}
