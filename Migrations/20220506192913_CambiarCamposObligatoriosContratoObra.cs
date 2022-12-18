using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CambiarCamposObligatoriosContratoObra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreCliente",
                table: "Congresses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(199), new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(209) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 29, 12, 878, DateTimeKind.Local).AddTicks(9573), new DateTime(2022, 5, 6, 21, 29, 12, 878, DateTimeKind.Local).AddTicks(9875) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(226), new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(228) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreCliente",
                table: "Congresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
