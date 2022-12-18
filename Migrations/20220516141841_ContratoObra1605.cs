using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ContratoObra1605 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoPostalObra",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 16, 16, 18, 40, 893, DateTimeKind.Local).AddTicks(2589), new DateTime(2022, 5, 16, 16, 18, 40, 893, DateTimeKind.Local).AddTicks(2598) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 16, 16, 18, 40, 893, DateTimeKind.Local).AddTicks(2003), new DateTime(2022, 5, 16, 16, 18, 40, 893, DateTimeKind.Local).AddTicks(2288) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 16, 16, 18, 40, 893, DateTimeKind.Local).AddTicks(2615), new DateTime(2022, 5, 16, 16, 18, 40, 893, DateTimeKind.Local).AddTicks(2618) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoPostalObra",
                table: "Congresses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3801), new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3809) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(2554), new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3233) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3825), new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3828) });
        }
    }
}
