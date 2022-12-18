using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class asistentefechaactualizacioncargonullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaActualizacionCargo",
                table: "Asistente",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5806), new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5816) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5019), new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5394) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5838), new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5841) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaActualizacionCargo",
                table: "Asistente",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4541), new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4551) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(3719), new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4109) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4571), new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4575) });
        }
    }
}
