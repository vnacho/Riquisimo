using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class estadoPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoPedido",
                table: "CompraPedidos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6367), new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6382) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(5203), new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(5767) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6402), new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6405) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoPedido",
                table: "CompraPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2719), new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2729) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(1613), new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2150) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2750), new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2753) });
        }
    }
}
