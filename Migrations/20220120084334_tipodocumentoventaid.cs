using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tipodocumentoventaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoDocumentoVentaId",
                table: "VentaPedidos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 20, 9, 43, 33, 893, DateTimeKind.Local).AddTicks(701), new DateTime(2022, 1, 20, 9, 43, 33, 893, DateTimeKind.Local).AddTicks(711) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 20, 9, 43, 33, 893, DateTimeKind.Local).AddTicks(58), new DateTime(2022, 1, 20, 9, 43, 33, 893, DateTimeKind.Local).AddTicks(365) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 20, 9, 43, 33, 893, DateTimeKind.Local).AddTicks(728), new DateTime(2022, 1, 20, 9, 43, 33, 893, DateTimeKind.Local).AddTicks(731) });

            migrationBuilder.CreateIndex(
                name: "IX_VentaPedidos_TipoDocumentoVentaId",
                table: "VentaPedidos",
                column: "TipoDocumentoVentaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VentaPedidos_TiposDocumentoVenta_TipoDocumentoVentaId",
                table: "VentaPedidos",
                column: "TipoDocumentoVentaId",
                principalTable: "TiposDocumentoVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentaPedidos_TiposDocumentoVenta_TipoDocumentoVentaId",
                table: "VentaPedidos");

            migrationBuilder.DropIndex(
                name: "IX_VentaPedidos_TipoDocumentoVentaId",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoVentaId",
                table: "VentaPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2898), new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2906) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2287), new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2581) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2922), new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2925) });
        }
    }
}
