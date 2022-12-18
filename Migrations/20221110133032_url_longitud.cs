using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class url_longitud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Congresses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5703), new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5714) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(4943), new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5268) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5733), new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5735) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Congresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5450), new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5460) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(4809), new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5115) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5479), new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5482) });
        }
    }
}
