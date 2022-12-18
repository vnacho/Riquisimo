using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ShowDataOnInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowCostCenterInfoOnInvoice",
                table: "Registrations",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCostCenterInfoOnInvoice",
                table: "Expenses",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCostCenterInfoOnInvoice",
                table: "Accommodations",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 12, 16, 3, 51, DateTimeKind.Local).AddTicks(1647), new DateTime(2020, 5, 4, 12, 16, 3, 51, DateTimeKind.Local).AddTicks(1657) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 12, 16, 3, 51, DateTimeKind.Local).AddTicks(530), new DateTime(2020, 5, 4, 12, 16, 3, 51, DateTimeKind.Local).AddTicks(1068) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 12, 16, 3, 51, DateTimeKind.Local).AddTicks(1677), new DateTime(2020, 5, 4, 12, 16, 3, 51, DateTimeKind.Local).AddTicks(1681) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowCostCenterInfoOnInvoice",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "ShowCostCenterInfoOnInvoice",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ShowCostCenterInfoOnInvoice",
                table: "Accommodations");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8392), new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8403) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(7264), new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(7818) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8424), new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8427) });
        }
    }
}
