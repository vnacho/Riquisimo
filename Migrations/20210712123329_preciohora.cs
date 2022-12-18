using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class preciohora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioHora",
                table: "Personal",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6052), new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6065) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(4835), new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(5423) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6087), new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6091) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioHora",
                table: "Personal");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5546), new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5557) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(4348), new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(4924) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5576), new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5580) });
        }
    }
}
