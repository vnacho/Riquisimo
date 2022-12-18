using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class addorigen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddOrigen",
                table: "TiposCentroCoste",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 9, 52, 25, 948, DateTimeKind.Local).AddTicks(1160), new DateTime(2021, 9, 30, 9, 52, 25, 948, DateTimeKind.Local).AddTicks(1170) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 9, 52, 25, 948, DateTimeKind.Local).AddTicks(371), new DateTime(2021, 9, 30, 9, 52, 25, 948, DateTimeKind.Local).AddTicks(744) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 9, 52, 25, 948, DateTimeKind.Local).AddTicks(1191), new DateTime(2021, 9, 30, 9, 52, 25, 948, DateTimeKind.Local).AddTicks(1194) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddOrigen",
                table: "TiposCentroCoste");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 23, 19, 19, 41, 990, DateTimeKind.Local).AddTicks(6779), new DateTime(2021, 9, 23, 19, 19, 41, 990, DateTimeKind.Local).AddTicks(6789) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 23, 19, 19, 41, 990, DateTimeKind.Local).AddTicks(5821), new DateTime(2021, 9, 23, 19, 19, 41, 990, DateTimeKind.Local).AddTicks(6355) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 23, 19, 19, 41, 990, DateTimeKind.Local).AddTicks(6816), new DateTime(2021, 9, 23, 19, 19, 41, 990, DateTimeKind.Local).AddTicks(6819) });
        }
    }
}
