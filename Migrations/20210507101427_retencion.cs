using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class retencion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Retencion_Porcentaje",
                table: "CompraFacturas",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5895), new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5909) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(4500), new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5065) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5932), new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5935) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Retencion_Porcentaje",
                table: "CompraFacturas",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3949), new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3959) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(2800), new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3355) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3977), new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3981) });
        }
    }
}
