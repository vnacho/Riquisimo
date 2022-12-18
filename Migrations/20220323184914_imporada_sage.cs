using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class imporada_sage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ImportadaSage",
                table: "VentaFacturas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6204), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6219) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(5298), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(5742) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6247), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6252) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportadaSage",
                table: "VentaFacturas");

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
    }
}
