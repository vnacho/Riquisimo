using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class datosconexioncomite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatabaseComite",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabasePasswordComite",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabasePrefixComite",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseServerComite",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseUserComite",
                table: "Congresses",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4037), new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4045) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(3356), new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(3696) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4063), new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4065) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseComite",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabasePasswordComite",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabasePrefixComite",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabaseServerComite",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabaseUserComite",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2665), new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2673) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2037), new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2334) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2692), new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2695) });
        }
    }
}
