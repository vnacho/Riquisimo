using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tiene_tiempo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TieneTiempo",
                table: "VentaPedidoLineas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TieneTiempo",
                table: "VentaFacturaLineas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TieneTiempo",
                table: "VentaAlbaranLineas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1760), new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1770) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1083), new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1412) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1789), new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1792) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieneTiempo",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "TieneTiempo",
                table: "VentaFacturaLineas");

            migrationBuilder.DropColumn(
                name: "TieneTiempo",
                table: "VentaAlbaranLineas");

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
    }
}
