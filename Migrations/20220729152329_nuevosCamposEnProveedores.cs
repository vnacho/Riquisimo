using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class nuevosCamposEnProveedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RETENCION",
                table: "Proveedores",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "MODO_RET",
                table: "Proveedores",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TIPO_RET",
                table: "Proveedores",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(622), new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(632) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(1), new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(302) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(649), new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(652) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MODO_RET",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "TIPO_RET",
                table: "Proveedores");

            migrationBuilder.AlterColumn<bool>(
                name: "RETENCION",
                table: "Proveedores",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

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
    }
}
