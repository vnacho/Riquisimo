using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class textodescripcionampliada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TextoDescripcionAmpliada",
                table: "VentaPedidoLineas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextoDescripcionAmpliada",
                table: "VentaFacturaLineas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextoDescripcionAmpliada",
                table: "VentaAlbaranLineas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3552), new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3562) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(2916), new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3224) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3581), new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3583) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextoDescripcionAmpliada",
                table: "VentaPedidoLineas");

            migrationBuilder.DropColumn(
                name: "TextoDescripcionAmpliada",
                table: "VentaFacturaLineas");

            migrationBuilder.DropColumn(
                name: "TextoDescripcionAmpliada",
                table: "VentaAlbaranLineas");

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
    }
}
