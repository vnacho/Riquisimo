using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class estadoAlbaran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoAlbaran",
                table: "CompraAlbaranes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 57, 23, 752, DateTimeKind.Local).AddTicks(8749), new DateTime(2021, 1, 7, 11, 57, 23, 752, DateTimeKind.Local).AddTicks(8760) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 57, 23, 752, DateTimeKind.Local).AddTicks(7274), new DateTime(2021, 1, 7, 11, 57, 23, 752, DateTimeKind.Local).AddTicks(7845) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 57, 23, 752, DateTimeKind.Local).AddTicks(8781), new DateTime(2021, 1, 7, 11, 57, 23, 752, DateTimeKind.Local).AddTicks(8784) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoAlbaran",
                table: "CompraAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 46, 33, 621, DateTimeKind.Local).AddTicks(58), new DateTime(2021, 1, 7, 11, 46, 33, 621, DateTimeKind.Local).AddTicks(68) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 46, 33, 620, DateTimeKind.Local).AddTicks(8896), new DateTime(2021, 1, 7, 11, 46, 33, 620, DateTimeKind.Local).AddTicks(9457) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 46, 33, 621, DateTimeKind.Local).AddTicks(88), new DateTime(2021, 1, 7, 11, 46, 33, 621, DateTimeKind.Local).AddTicks(91) });
        }
    }
}
