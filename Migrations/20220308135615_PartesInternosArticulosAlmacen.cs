using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class PartesInternosArticulosAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4918), new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4927) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4309), new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4604) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4943), new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4945) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 1, 16, 52, 36, 408, DateTimeKind.Local).AddTicks(1871), new DateTime(2022, 3, 1, 16, 52, 36, 408, DateTimeKind.Local).AddTicks(1879) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 1, 16, 52, 36, 408, DateTimeKind.Local).AddTicks(1256), new DateTime(2022, 3, 1, 16, 52, 36, 408, DateTimeKind.Local).AddTicks(1556) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 1, 16, 52, 36, 408, DateTimeKind.Local).AddTicks(1896), new DateTime(2022, 3, 1, 16, 52, 36, 408, DateTimeKind.Local).AddTicks(1899) });
        }
    }
}
