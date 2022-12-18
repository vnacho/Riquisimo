using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class incluirdatosevento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncluirDatosEvento",
                table: "VentaFacturas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4960), new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4970) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4303), new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4618) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4987), new DateTime(2022, 3, 28, 11, 27, 59, 864, DateTimeKind.Local).AddTicks(4990) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncluirDatosEvento",
                table: "VentaFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 25, 9, 55, 17, 607, DateTimeKind.Local).AddTicks(4734), new DateTime(2022, 3, 25, 9, 55, 17, 607, DateTimeKind.Local).AddTicks(4742) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 25, 9, 55, 17, 607, DateTimeKind.Local).AddTicks(4108), new DateTime(2022, 3, 25, 9, 55, 17, 607, DateTimeKind.Local).AddTicks(4410) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 25, 9, 55, 17, 607, DateTimeKind.Local).AddTicks(4759), new DateTime(2022, 3, 25, 9, 55, 17, 607, DateTimeKind.Local).AddTicks(4762) });
        }
    }
}
