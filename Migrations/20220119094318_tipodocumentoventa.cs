using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tipodocumentoventa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposDocumentoVenta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDocumentoVenta", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2898), new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2906) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2287), new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2581) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2922), new DateTime(2022, 1, 19, 10, 43, 18, 348, DateTimeKind.Local).AddTicks(2925) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiposDocumentoVenta");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2273), new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2281) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(1622), new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(1941) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2297), new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2300) });
        }
    }
}
