using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class DefaultValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Serie",
                table: "Registrations",
                nullable: true,
                defaultValue: "I ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Product",
                table: "Registrations",
                nullable: true,
                defaultValue: "70100",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Serie",
                table: "Accommodations",
                nullable: true,
                defaultValue: "A ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Product",
                table: "Accommodations",
                nullable: true,
                defaultValue: "70800",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1654), new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1668) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(551), new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1094) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1688), new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1691) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Serie",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "I ");

            migrationBuilder.AlterColumn<string>(
                name: "Product",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "70100");

            migrationBuilder.AlterColumn<string>(
                name: "Serie",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "A ");

            migrationBuilder.AlterColumn<string>(
                name: "Product",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "70800");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9901), new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9911) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(8761), new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9322) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9932), new DateTime(2020, 4, 28, 16, 47, 15, 504, DateTimeKind.Local).AddTicks(9935) });
        }
    }
}
