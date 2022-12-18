using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class pagada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Pagada",
                table: "CompraFacturas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 7, 11, 44, 35, 873, DateTimeKind.Local).AddTicks(7293), new DateTime(2021, 4, 7, 11, 44, 35, 873, DateTimeKind.Local).AddTicks(7302) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 7, 11, 44, 35, 873, DateTimeKind.Local).AddTicks(6144), new DateTime(2021, 4, 7, 11, 44, 35, 873, DateTimeKind.Local).AddTicks(6698) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 7, 11, 44, 35, 873, DateTimeKind.Local).AddTicks(7324), new DateTime(2021, 4, 7, 11, 44, 35, 873, DateTimeKind.Local).AddTicks(7327) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pagada",
                table: "CompraFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 17, 10, 58, 26, 365, DateTimeKind.Local).AddTicks(3950), new DateTime(2021, 3, 17, 10, 58, 26, 365, DateTimeKind.Local).AddTicks(3960) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 17, 10, 58, 26, 365, DateTimeKind.Local).AddTicks(1729), new DateTime(2021, 3, 17, 10, 58, 26, 365, DateTimeKind.Local).AddTicks(3262) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 17, 10, 58, 26, 365, DateTimeKind.Local).AddTicks(3983), new DateTime(2021, 3, 17, 10, 58, 26, 365, DateTimeKind.Local).AddTicks(3986) });
        }
    }
}
