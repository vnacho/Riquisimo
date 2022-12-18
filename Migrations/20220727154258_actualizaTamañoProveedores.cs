using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class actualizaTamañoProveedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CUENTACONTABLE",
                table: "Proveedores",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 27, 17, 42, 57, 902, DateTimeKind.Local).AddTicks(6014), new DateTime(2022, 7, 27, 17, 42, 57, 902, DateTimeKind.Local).AddTicks(6023) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 27, 17, 42, 57, 902, DateTimeKind.Local).AddTicks(5412), new DateTime(2022, 7, 27, 17, 42, 57, 902, DateTimeKind.Local).AddTicks(5702) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 27, 17, 42, 57, 902, DateTimeKind.Local).AddTicks(6040), new DateTime(2022, 7, 27, 17, 42, 57, 902, DateTimeKind.Local).AddTicks(6043) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CUENTACONTABLE",
                table: "Proveedores",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5262), new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5273) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(4662), new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(4948) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5289), new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5291) });
        }
    }
}
