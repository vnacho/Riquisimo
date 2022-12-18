using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CampoFechaNacimientoEnPersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Personal",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2665), new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2673) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2037), new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2334) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2692), new DateTime(2022, 7, 19, 20, 27, 58, 537, DateTimeKind.Local).AddTicks(2695) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Personal");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1782), new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1804) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1016), new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1388) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1821), new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1824) });
        }
    }
}
