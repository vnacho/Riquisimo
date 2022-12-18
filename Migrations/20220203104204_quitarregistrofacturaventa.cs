using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class quitarregistrofacturaventa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Registro",
                table: "VentaFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4151), new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4159) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(3502), new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(3813) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4176), new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4179) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Registro",
                table: "VentaFacturas",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4514), new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4521) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(3872), new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4538), new DateTime(2022, 2, 2, 18, 42, 17, 714, DateTimeKind.Local).AddTicks(4540) });
        }
    }
}
