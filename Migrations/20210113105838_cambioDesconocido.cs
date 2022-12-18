using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class cambioDesconocido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompraAlbaranLineaId",
                table: "CompraFacturaLineas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 13, 11, 58, 37, 763, DateTimeKind.Local).AddTicks(6432), new DateTime(2021, 1, 13, 11, 58, 37, 763, DateTimeKind.Local).AddTicks(6441) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 13, 11, 58, 37, 763, DateTimeKind.Local).AddTicks(5325), new DateTime(2021, 1, 13, 11, 58, 37, 763, DateTimeKind.Local).AddTicks(5860) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 13, 11, 58, 37, 763, DateTimeKind.Local).AddTicks(6461), new DateTime(2021, 1, 13, 11, 58, 37, 763, DateTimeKind.Local).AddTicks(6464) });

            migrationBuilder.CreateIndex(
                name: "IX_CompraFacturaLineas_CompraAlbaranLineaId",
                table: "CompraFacturaLineas",
                column: "CompraAlbaranLineaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraFacturaLineas_CompraAlbaranLineas_CompraAlbaranLineaId",
                table: "CompraFacturaLineas",
                column: "CompraAlbaranLineaId",
                principalTable: "CompraAlbaranLineas",
                principalColumn: "IdAlbaranLinea",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraFacturaLineas_CompraAlbaranLineas_CompraAlbaranLineaId",
                table: "CompraFacturaLineas");

            migrationBuilder.DropIndex(
                name: "IX_CompraFacturaLineas_CompraAlbaranLineaId",
                table: "CompraFacturaLineas");

            migrationBuilder.DropColumn(
                name: "CompraAlbaranLineaId",
                table: "CompraFacturaLineas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3521), new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3533) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(1936), new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(2799) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3556), new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3559) });
        }
    }
}
