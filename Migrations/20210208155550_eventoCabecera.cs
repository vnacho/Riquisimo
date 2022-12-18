using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class eventoCabecera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoEvento",
                table: "CompraPedidos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreEvento",
                table: "CompraPedidos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoEvento",
                table: "CompraFacturas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreEvento",
                table: "CompraFacturas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoEvento",
                table: "CompraAlbaranes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreEvento",
                table: "CompraAlbaranes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7761), new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(6549), new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7112) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7795), new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7798) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoEvento",
                table: "CompraPedidos");

            migrationBuilder.DropColumn(
                name: "NombreEvento",
                table: "CompraPedidos");

            migrationBuilder.DropColumn(
                name: "CodigoEvento",
                table: "CompraFacturas");

            migrationBuilder.DropColumn(
                name: "NombreEvento",
                table: "CompraFacturas");

            migrationBuilder.DropColumn(
                name: "CodigoEvento",
                table: "CompraAlbaranes");

            migrationBuilder.DropColumn(
                name: "NombreEvento",
                table: "CompraAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 18, 10, 30, 2, 281, DateTimeKind.Local).AddTicks(839), new DateTime(2021, 1, 18, 10, 30, 2, 281, DateTimeKind.Local).AddTicks(849) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 18, 10, 30, 2, 280, DateTimeKind.Local).AddTicks(9309), new DateTime(2021, 1, 18, 10, 30, 2, 280, DateTimeKind.Local).AddTicks(9869) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 18, 10, 30, 2, 281, DateTimeKind.Local).AddTicks(868), new DateTime(2021, 1, 18, 10, 30, 2, 281, DateTimeKind.Local).AddTicks(871) });
        }
    }
}
