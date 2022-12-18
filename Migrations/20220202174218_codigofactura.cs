using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class codigofactura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "VentaFacturas");

            migrationBuilder.AddColumn<int>(
                name: "CodigoFactura",
                table: "VentaFacturas",
                maxLength: 24,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4514), new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4521) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(3872), new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4538), new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4540) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoFactura",
                table: "VentaFacturas");

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "VentaFacturas",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 16, 57, 46, 596, DateTimeKind.Local).AddTicks(3174), new DateTime(2022, 2, 2, 16, 57, 46, 596, DateTimeKind.Local).AddTicks(3182) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 16, 57, 46, 596, DateTimeKind.Local).AddTicks(2515), new DateTime(2022, 2, 2, 16, 57, 46, 596, DateTimeKind.Local).AddTicks(2830) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 16, 57, 46, 596, DateTimeKind.Local).AddTicks(3199), new DateTime(2022, 2, 2, 16, 57, 46, 596, DateTimeKind.Local).AddTicks(3201) });
        }
    }
}
