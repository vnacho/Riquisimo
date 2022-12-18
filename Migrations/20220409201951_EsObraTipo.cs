using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class EsObraTipo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoCongress",
                table: "Congresses",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 22, 19, 51, 440, DateTimeKind.Local).AddTicks(377), new DateTime(2022, 4, 9, 22, 19, 51, 440, DateTimeKind.Local).AddTicks(385) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 22, 19, 51, 439, DateTimeKind.Local).AddTicks(9762), new DateTime(2022, 4, 9, 22, 19, 51, 440, DateTimeKind.Local).AddTicks(62) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 22, 19, 51, 440, DateTimeKind.Local).AddTicks(403), new DateTime(2022, 4, 9, 22, 19, 51, 440, DateTimeKind.Local).AddTicks(405) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoCongress",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5964), new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5973) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5372), new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5658) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5990), new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5993) });
        }
    }
}
