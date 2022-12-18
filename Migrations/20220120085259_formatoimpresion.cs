using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class formatoimpresion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormatoImpresion",
                table: "VentaPedidos",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatoImpresion",
                table: "VentaPedidos");

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
        }
    }
}
