using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class nif_restauracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NIF",
                table: "Restauraciones",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIF",
                table: "Restauraciones");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9565), new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9574) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(8924), new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9230) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9592), new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9595) });
        }
    }
}
