using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AccountRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessAdmin",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessBudgetControl",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessCollaborations",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessCongress",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9901), new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9911) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(8761), new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9322) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9932), new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9935) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessAdmin",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessBudgetControl",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessCollaborations",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessCongress",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1356), new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1367) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(190), new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(762) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1387), new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1390) });
        }
    }
}
