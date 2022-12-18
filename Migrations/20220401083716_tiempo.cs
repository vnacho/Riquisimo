using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tiempo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Tiempo",
                table: "VentaPedidoLineas",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tiempo",
                table: "VentaFacturaLineas",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tiempo",
                table: "VentaAlbaranLineas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 10, 37, 16, 382, DateTimeKind.Local).AddTicks(2680), new DateTime(2022, 4, 1, 10, 37, 16, 382, DateTimeKind.Local).AddTicks(2690) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 10, 37, 16, 382, DateTimeKind.Local).AddTicks(2032), new DateTime(2022, 4, 1, 10, 37, 16, 382, DateTimeKind.Local).AddTicks(2345) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 10, 37, 16, 382, DateTimeKind.Local).AddTicks(2710), new DateTime(2022, 4, 1, 10, 37, 16, 382, DateTimeKind.Local).AddTicks(2713) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tiempo",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "Tiempo",
                table: "VentaFacturaLineas");

            migrationBuilder.DropColumn(
                name: "Tiempo",
                table: "VentaAlbaranLineas");

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
    }
}
