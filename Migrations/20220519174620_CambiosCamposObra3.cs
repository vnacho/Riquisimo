using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CambiosCamposObra3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaContratoFin",
                table: "ContratoObras",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 19, 19, 46, 20, 443, DateTimeKind.Local).AddTicks(6168), new DateTime(2022, 5, 19, 19, 46, 20, 443, DateTimeKind.Local).AddTicks(6177) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 19, 19, 46, 20, 443, DateTimeKind.Local).AddTicks(5548), new DateTime(2022, 5, 19, 19, 46, 20, 443, DateTimeKind.Local).AddTicks(5845) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 19, 19, 46, 20, 443, DateTimeKind.Local).AddTicks(6193), new DateTime(2022, 5, 19, 19, 46, 20, 443, DateTimeKind.Local).AddTicks(6196) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaContratoFin",
                table: "ContratoObras",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 19, 14, 40, 28, 648, DateTimeKind.Local).AddTicks(8806), new DateTime(2022, 5, 19, 14, 40, 28, 648, DateTimeKind.Local).AddTicks(8814) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 19, 14, 40, 28, 648, DateTimeKind.Local).AddTicks(8208), new DateTime(2022, 5, 19, 14, 40, 28, 648, DateTimeKind.Local).AddTicks(8491) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 19, 14, 40, 28, 648, DateTimeKind.Local).AddTicks(8833), new DateTime(2022, 5, 19, 14, 40, 28, 648, DateTimeKind.Local).AddTicks(8836) });
        }
    }
}
