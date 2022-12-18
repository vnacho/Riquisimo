using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class porcentajedistribucion52 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PorcentajeDistribucion",
                table: "TiposCentroCoste",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8715), new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8725) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(7325), new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(7991) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8761), new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8765) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PorcentajeDistribucion",
                table: "TiposCentroCoste",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 18, 35, 43, 661, DateTimeKind.Local).AddTicks(9796), new DateTime(2021, 6, 15, 18, 35, 43, 661, DateTimeKind.Local).AddTicks(9806) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 18, 35, 43, 661, DateTimeKind.Local).AddTicks(8366), new DateTime(2021, 6, 15, 18, 35, 43, 661, DateTimeKind.Local).AddTicks(8974) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 18, 35, 43, 661, DateTimeKind.Local).AddTicks(9828), new DateTime(2021, 6, 15, 18, 35, 43, 661, DateTimeKind.Local).AddTicks(9831) });
        }
    }
}
