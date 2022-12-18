using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class parametros_formato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Formato",
                table: "Parametros",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(799), new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(808) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(180), new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(478) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(827), new DateTime(2022, 5, 27, 13, 38, 0, 120, DateTimeKind.Local).AddTicks(829) });

            migrationBuilder.UpdateData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA",
                column: "Formato",
                value: "dd/MM/yyyy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formato",
                table: "Parametros");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4482), new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4493) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(3797), new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4147) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4511), new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4514) });
        }
    }
}
