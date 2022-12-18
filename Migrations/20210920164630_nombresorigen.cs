using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class nombresorigen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreNivelAnalitico1",
                table: "Origen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreNivelAnalitico2",
                table: "Origen",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 20, 18, 46, 29, 602, DateTimeKind.Local).AddTicks(7204), new DateTime(2021, 9, 20, 18, 46, 29, 602, DateTimeKind.Local).AddTicks(7215) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 20, 18, 46, 29, 602, DateTimeKind.Local).AddTicks(6370), new DateTime(2021, 9, 20, 18, 46, 29, 602, DateTimeKind.Local).AddTicks(6747) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 20, 18, 46, 29, 602, DateTimeKind.Local).AddTicks(7237), new DateTime(2021, 9, 20, 18, 46, 29, 602, DateTimeKind.Local).AddTicks(7241) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreNivelAnalitico1",
                table: "Origen");

            migrationBuilder.DropColumn(
                name: "NombreNivelAnalitico2",
                table: "Origen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(412), new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(423) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 52, 53, 658, DateTimeKind.Local).AddTicks(9579), new DateTime(2021, 9, 14, 10, 52, 53, 658, DateTimeKind.Local).AddTicks(9950) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(445), new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(448) });
        }
    }
}
