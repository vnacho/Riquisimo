using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class compraPedidoLineaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompraPedidoLineaId",
                table: "CompraFacturaLineas",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_CompraFacturaLineas_CompraPedidoLineaId",
                table: "CompraFacturaLineas",
                column: "CompraPedidoLineaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraFacturaLineas_CompraPedidoLineas_CompraPedidoLineaId",
                table: "CompraFacturaLineas",
                column: "CompraPedidoLineaId",
                principalTable: "CompraPedidoLineas",
                principalColumn: "IdPedidoLinea",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraFacturaLineas_CompraPedidoLineas_CompraPedidoLineaId",
                table: "CompraFacturaLineas");

            migrationBuilder.DropIndex(
                name: "IX_CompraFacturaLineas_CompraPedidoLineaId",
                table: "CompraFacturaLineas");

            migrationBuilder.DropColumn(
                name: "CompraPedidoLineaId",
                table: "CompraFacturaLineas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 15, 17, 38, 24, 751, DateTimeKind.Local).AddTicks(5073), new DateTime(2021, 1, 15, 17, 38, 24, 751, DateTimeKind.Local).AddTicks(5083) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 15, 17, 38, 24, 751, DateTimeKind.Local).AddTicks(3920), new DateTime(2021, 1, 15, 17, 38, 24, 751, DateTimeKind.Local).AddTicks(4477) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 15, 17, 38, 24, 751, DateTimeKind.Local).AddTicks(5104), new DateTime(2021, 1, 15, 17, 38, 24, 751, DateTimeKind.Local).AddTicks(5107) });
        }
    }
}
