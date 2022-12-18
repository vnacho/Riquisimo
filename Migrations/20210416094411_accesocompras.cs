using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class accesocompras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IVA_Porcentaje",
                table: "CompraFacturaLineas",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "AccessCompras",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3949), new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3959) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(2800), new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3355) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3977), new DateTime(2021, 4, 16, 11, 44, 10, 534, DateTimeKind.Local).AddTicks(3981) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCompras",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "IVA_Porcentaje",
                table: "CompraFacturaLineas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 14, 17, 57, 19, 960, DateTimeKind.Local).AddTicks(5475), new DateTime(2021, 4, 14, 17, 57, 19, 960, DateTimeKind.Local).AddTicks(5489) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 14, 17, 57, 19, 960, DateTimeKind.Local).AddTicks(4134), new DateTime(2021, 4, 14, 17, 57, 19, 960, DateTimeKind.Local).AddTicks(4716) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 4, 14, 17, 57, 19, 960, DateTimeKind.Local).AddTicks(5512), new DateTime(2021, 4, 14, 17, 57, 19, 960, DateTimeKind.Local).AddTicks(5516) });
        }
    }
}
