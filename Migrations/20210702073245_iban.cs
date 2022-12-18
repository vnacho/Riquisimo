using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class iban : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "Personal",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5546), new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5557) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(4348), new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(4924) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5576), new DateTime(2021, 7, 2, 9, 32, 44, 976, DateTimeKind.Local).AddTicks(5580) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "Personal",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(461), new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(474) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 30, 12, 26, 3, 951, DateTimeKind.Local).AddTicks(9143), new DateTime(2021, 6, 30, 12, 26, 3, 951, DateTimeKind.Local).AddTicks(9829) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(496), new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(499) });
        }
    }
}
