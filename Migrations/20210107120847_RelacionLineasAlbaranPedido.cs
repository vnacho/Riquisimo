using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class RelacionLineasAlbaranPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompraPedidoLineaId",
                table: "CompraAlbaranLineas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2268), new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2281) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(642), new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(1552) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2306), new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2310) });

            migrationBuilder.CreateIndex(
                name: "IX_CompraAlbaranLineas_CompraPedidoLineaId",
                table: "CompraAlbaranLineas",
                column: "CompraPedidoLineaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraAlbaranLineas_CompraPedidoLineas_CompraPedidoLineaId",
                table: "CompraAlbaranLineas",
                column: "CompraPedidoLineaId",
                principalTable: "CompraPedidoLineas",
                principalColumn: "IdPedidoLinea",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraAlbaranLineas_CompraPedidoLineas_CompraPedidoLineaId",
                table: "CompraAlbaranLineas");

            migrationBuilder.DropIndex(
                name: "IX_CompraAlbaranLineas_CompraPedidoLineaId",
                table: "CompraAlbaranLineas");

            migrationBuilder.DropColumn(
                name: "CompraPedidoLineaId",
                table: "CompraAlbaranLineas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 5, 31, 302, DateTimeKind.Local).AddTicks(7803), new DateTime(2021, 1, 7, 13, 5, 31, 302, DateTimeKind.Local).AddTicks(7813) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 5, 31, 302, DateTimeKind.Local).AddTicks(6582), new DateTime(2021, 1, 7, 13, 5, 31, 302, DateTimeKind.Local).AddTicks(7164) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 5, 31, 302, DateTimeKind.Local).AddTicks(7833), new DateTime(2021, 1, 7, 13, 5, 31, 302, DateTimeKind.Local).AddTicks(7836) });
        }
    }
}
