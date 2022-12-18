using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class rolventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessVentas",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(415), new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(427) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 11, 19, 11, 55, 58, 795, DateTimeKind.Local).AddTicks(8973), new DateTime(2021, 11, 19, 11, 55, 58, 795, DateTimeKind.Local).AddTicks(9838) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(450), new DateTime(2021, 11, 19, 11, 55, 58, 796, DateTimeKind.Local).AddTicks(454) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessVentas",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7254), new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7267) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(6338), new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(6775) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7290), new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7294) });
        }
    }
}
