using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ArticulosAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2097), new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2105) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(1459), new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(1761) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2121), new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2123) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8014), new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8023) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(7360), new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(7675) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8040), new DateTime(2022, 2, 17, 10, 51, 38, 860, DateTimeKind.Local).AddTicks(8043) });
        }
    }
}
