using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class usuario_compras_ventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "VentaAlbaranes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "CompraPedidos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "CompraFacturas",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "CompraAlbaranes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 25, 9, 9, 36, 900, DateTimeKind.Local).AddTicks(3654), new DateTime(2022, 3, 25, 9, 9, 36, 900, DateTimeKind.Local).AddTicks(3663) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 25, 9, 9, 36, 900, DateTimeKind.Local).AddTicks(3007), new DateTime(2022, 3, 25, 9, 9, 36, 900, DateTimeKind.Local).AddTicks(3320) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 25, 9, 9, 36, 900, DateTimeKind.Local).AddTicks(3680), new DateTime(2022, 3, 25, 9, 9, 36, 900, DateTimeKind.Local).AddTicks(3682) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "CompraPedidos");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "CompraFacturas");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "CompraAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6204), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6219) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(5298), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(5742) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6247), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6252) });
        }
    }
}
