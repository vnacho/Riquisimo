using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class descripcionampliada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DescripcionAmpliada",
                table: "VentaPedidoLineas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DescripcionAmpliada",
                table: "VentaFacturaLineas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DescripcionAmpliada",
                table: "VentaAlbaranLineas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 8, 51, 815, DateTimeKind.Local).AddTicks(4348), new DateTime(2022, 1, 25, 11, 8, 51, 815, DateTimeKind.Local).AddTicks(4358) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 8, 51, 815, DateTimeKind.Local).AddTicks(3718), new DateTime(2022, 1, 25, 11, 8, 51, 815, DateTimeKind.Local).AddTicks(4018) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 8, 51, 815, DateTimeKind.Local).AddTicks(4376), new DateTime(2022, 1, 25, 11, 8, 51, 815, DateTimeKind.Local).AddTicks(4378) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescripcionAmpliada",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "DescripcionAmpliada",
                table: "VentaFacturaLineas");

            migrationBuilder.DropColumn(
                name: "DescripcionAmpliada",
                table: "VentaAlbaranLineas");

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
    }
}
