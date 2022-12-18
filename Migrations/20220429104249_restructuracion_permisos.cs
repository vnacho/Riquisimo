using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class restructuracion_permisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessAdmin",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessBudgetControl",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessCompras",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessInvoices",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessMaintenance",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccessVentas",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1506), new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1528) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(848), new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1166) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1547), new DateTime(2022, 4, 29, 12, 42, 48, 603, DateTimeKind.Local).AddTicks(1550) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessAdmin",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessBudgetControl",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessCompras",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessInvoices",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessMaintenance",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AccessVentas",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 28, 18, 3, 34, 54, DateTimeKind.Local).AddTicks(393), new DateTime(2022, 4, 28, 18, 3, 34, 54, DateTimeKind.Local).AddTicks(402) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 28, 18, 3, 34, 53, DateTimeKind.Local).AddTicks(9749), new DateTime(2022, 4, 28, 18, 3, 34, 54, DateTimeKind.Local).AddTicks(56) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 28, 18, 3, 34, 54, DateTimeKind.Local).AddTicks(418), new DateTime(2022, 4, 28, 18, 3, 34, 54, DateTimeKind.Local).AddTicks(421) });
        }
    }
}
