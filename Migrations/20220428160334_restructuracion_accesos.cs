using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class restructuracion_accesos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PermisoAdministracion",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermisoCompras",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermisoControlPresupuestario",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermisoEventos",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermisoFacturacion",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermisoVentas",
                table: "Accounts",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermisoAdministracion",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PermisoCompras",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PermisoControlPresupuestario",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PermisoEventos",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PermisoFacturacion",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PermisoVentas",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 28, 17, 44, 39, 586, DateTimeKind.Local).AddTicks(9209), new DateTime(2022, 4, 28, 17, 44, 39, 586, DateTimeKind.Local).AddTicks(9218) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 28, 17, 44, 39, 586, DateTimeKind.Local).AddTicks(8532), new DateTime(2022, 4, 28, 17, 44, 39, 586, DateTimeKind.Local).AddTicks(8873) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 28, 17, 44, 39, 586, DateTimeKind.Local).AddTicks(9236), new DateTime(2022, 4, 28, 17, 44, 39, 586, DateTimeKind.Local).AddTicks(9238) });
        }
    }
}
