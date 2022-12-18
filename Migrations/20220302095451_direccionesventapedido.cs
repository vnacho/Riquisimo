using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class direccionesventapedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poblacion",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1562), new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1571) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(934), new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1240) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1592), new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1595) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "Poblacion",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "VentaPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 23, 57, 117, DateTimeKind.Local).AddTicks(7713), new DateTime(2022, 3, 2, 10, 23, 57, 117, DateTimeKind.Local).AddTicks(7722) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 23, 57, 117, DateTimeKind.Local).AddTicks(7068), new DateTime(2022, 3, 2, 10, 23, 57, 117, DateTimeKind.Local).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 23, 57, 117, DateTimeKind.Local).AddTicks(7739), new DateTime(2022, 3, 2, 10, 23, 57, 117, DateTimeKind.Local).AddTicks(7741) });
        }
    }
}
