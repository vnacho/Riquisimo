using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class databaseFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Database",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabasePassword",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseServer",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseUser",
                table: "Congresses",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6572), new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6581) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(5478), new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6011) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6602), new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6606) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Database",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabasePassword",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabaseServer",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DatabaseUser",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6845), new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6859) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(5563), new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6140) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6882), new DateTime(2020, 5, 26, 13, 49, 38, 293, DateTimeKind.Local).AddTicks(6885) });
        }
    }
}
