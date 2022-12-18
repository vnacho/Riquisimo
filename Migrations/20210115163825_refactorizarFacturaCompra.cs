using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class refactorizarFacturaCompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fichero",
                table: "CompraFacturas");

            migrationBuilder.DropColumn(
                name: "TieneFichero",
                table: "CompraFacturas");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Fichero",
                table: "CompraFacturas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneFichero",
                table: "CompraFacturas",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }
    }
}
