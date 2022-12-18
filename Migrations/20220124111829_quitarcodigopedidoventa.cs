using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class quitarcodigopedidoventa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoPedido",
                table: "VentaPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7699), new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7707) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7024), new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7330) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7724), new DateTime(2022, 1, 24, 12, 18, 29, 149, DateTimeKind.Local).AddTicks(7727) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoPedido",
                table: "VentaPedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 20, 9, 52, 59, 48, DateTimeKind.Local).AddTicks(5177), new DateTime(2022, 1, 20, 9, 52, 59, 48, DateTimeKind.Local).AddTicks(5186) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 20, 9, 52, 59, 48, DateTimeKind.Local).AddTicks(4517), new DateTime(2022, 1, 20, 9, 52, 59, 48, DateTimeKind.Local).AddTicks(4819) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 20, 9, 52, 59, 48, DateTimeKind.Local).AddTicks(5203), new DateTime(2022, 1, 20, 9, 52, 59, 48, DateTimeKind.Local).AddTicks(5205) });
        }
    }
}
