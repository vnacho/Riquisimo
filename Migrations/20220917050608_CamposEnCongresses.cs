using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CamposEnCongresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEntregaEpi",
                table: "Personal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plataforma",
                table: "Congresses",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contraseña",
                table: "Congresses",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "Congresses",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "usuario",
                table: "Congresses",
                maxLength: 30,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9272), new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9285) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(8366), new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(8741) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9321), new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9325) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaEntregaEpi",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "Plataforma",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "contraseña",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "url",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "usuario",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 7, 12, 39, 9, 739, DateTimeKind.Local).AddTicks(7490), new DateTime(2022, 9, 7, 12, 39, 9, 739, DateTimeKind.Local).AddTicks(7501) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 7, 12, 39, 9, 739, DateTimeKind.Local).AddTicks(6870), new DateTime(2022, 9, 7, 12, 39, 9, 739, DateTimeKind.Local).AddTicks(7166) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 7, 12, 39, 9, 739, DateTimeKind.Local).AddTicks(7518), new DateTime(2022, 9, 7, 12, 39, 9, 739, DateTimeKind.Local).AddTicks(7520) });
        }
    }
}
