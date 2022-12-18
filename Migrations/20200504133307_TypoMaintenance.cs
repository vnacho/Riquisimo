using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class TypoMaintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessMaintenece",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "AccessMaintenance",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8602), new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8613) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(7419), new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(7998) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8635), new DateTime(2020, 5, 4, 15, 33, 6, 535, DateTimeKind.Local).AddTicks(8638) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessMaintenance",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "AccessMaintenece",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 16, 19, 314, DateTimeKind.Local).AddTicks(6896), new DateTime(2020, 5, 4, 15, 16, 19, 314, DateTimeKind.Local).AddTicks(6907) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 16, 19, 314, DateTimeKind.Local).AddTicks(5691), new DateTime(2020, 5, 4, 15, 16, 19, 314, DateTimeKind.Local).AddTicks(6276) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 4, 15, 16, 19, 314, DateTimeKind.Local).AddTicks(6931), new DateTime(2020, 5, 4, 15, 16, 19, 314, DateTimeKind.Local).AddTicks(6934) });
        }
    }
}
