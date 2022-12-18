using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class vendedor_ventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoOperario",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "NombreOperario",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "CodigoOperario",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "NombreOperario",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "CodigoOperario",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "NombreOperario",
                table: "VentaAlbaranes");

            migrationBuilder.AddColumn<string>(
                name: "CodigoVendedor",
                table: "VentaPedidos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreVendedor",
                table: "VentaPedidos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoVendedor",
                table: "VentaFacturas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreVendedor",
                table: "VentaFacturas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoVendedor",
                table: "VentaAlbaranes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreVendedor",
                table: "VentaAlbaranes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2036), new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2046) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(1347), new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(1658) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2065), new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2068) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoVendedor",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "NombreVendedor",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "CodigoVendedor",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "NombreVendedor",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "CodigoVendedor",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "NombreVendedor",
                table: "VentaAlbaranes");

            migrationBuilder.AddColumn<string>(
                name: "CodigoOperario",
                table: "VentaPedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreOperario",
                table: "VentaPedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoOperario",
                table: "VentaFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreOperario",
                table: "VentaFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoOperario",
                table: "VentaAlbaranes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreOperario",
                table: "VentaAlbaranes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9392), new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9404) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(8564), new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(8966) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9432), new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9436) });
        }
    }
}
