using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class iva_pedidos_albaranes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseImponible",
                table: "VentaPedidos",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoIVA",
                table: "VentaPedidoLineas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IVA_Porcentaje",
                table: "VentaPedidoLineas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoIVA",
                table: "VentaAlbaranLineas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IVA_Porcentaje",
                table: "VentaAlbaranLineas",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseImponible",
                table: "VentaAlbaranes",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8014), new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8023) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(7360), new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(7675) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8040), new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8043) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseImponible",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "CodigoTipoIVA",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "IVA_Porcentaje",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "CodigoTipoIVA",
                table: "VentaAlbaranLineas");

            migrationBuilder.DropColumn(
                name: "IVA_Porcentaje",
                table: "VentaAlbaranLineas");

            migrationBuilder.DropColumn(
                name: "BaseImponible",
                table: "VentaAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8569), new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8577) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(7965), new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8257) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8593), new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8595) });
        }
    }
}
