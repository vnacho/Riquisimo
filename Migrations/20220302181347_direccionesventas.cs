using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class direccionesventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineaEnvCli",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poblacion",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "VentaFacturas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "VentaAlbaranes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "VentaAlbaranes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineaEnvCli",
                table: "VentaAlbaranes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poblacion",
                table: "VentaAlbaranes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "VentaAlbaranes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 19, 13, 46, 956, DateTimeKind.Local).AddTicks(4537), new DateTime(2022, 3, 2, 19, 13, 46, 956, DateTimeKind.Local).AddTicks(4546) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 19, 13, 46, 956, DateTimeKind.Local).AddTicks(3885), new DateTime(2022, 3, 2, 19, 13, 46, 956, DateTimeKind.Local).AddTicks(4201) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 19, 13, 46, 956, DateTimeKind.Local).AddTicks(4568), new DateTime(2022, 3, 2, 19, 13, 46, 956, DateTimeKind.Local).AddTicks(4571) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "LineaEnvCli",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "Poblacion",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "VentaFacturas");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "LineaEnvCli",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "Poblacion",
                table: "VentaAlbaranes");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "VentaAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1562), new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1571) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(934), new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1240) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1592), new DateTime(2022, 3, 2, 10, 54, 51, 359, DateTimeKind.Local).AddTicks(1595) });
        }
    }
}
