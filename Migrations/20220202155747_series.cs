using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class series : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "VentaPedidos",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "VentaFacturas",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "VentaAlbaranes",
                maxLength: 2,
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Serie",
                table: "VentaPedidos");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "VentaAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8881), new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8890) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8237), new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8546) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8908), new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8910) });
        }
    }
}
