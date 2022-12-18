using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class codigosage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroFactura",
                table: "CompraFacturas",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CodigoSage",
                table: "CompraFacturas",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8898), new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8911) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(7715), new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8288) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8934), new DateTime(2021, 3, 2, 15, 1, 14, 436, DateTimeKind.Local).AddTicks(8937) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroFactura",
                table: "CompraFacturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoSage",
                table: "CompraFacturas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3706), new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3715) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(2470), new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3030) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3735), new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3738) });
        }
    }
}
