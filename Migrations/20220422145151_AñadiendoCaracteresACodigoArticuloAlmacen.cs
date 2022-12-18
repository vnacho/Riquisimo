using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AñadiendoCaracteresACodigoArticuloAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "ArticulosAlmacen",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 22, 16, 51, 51, 494, DateTimeKind.Local).AddTicks(4324), new DateTime(2022, 4, 22, 16, 51, 51, 494, DateTimeKind.Local).AddTicks(4332) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 22, 16, 51, 51, 494, DateTimeKind.Local).AddTicks(3698), new DateTime(2022, 4, 22, 16, 51, 51, 494, DateTimeKind.Local).AddTicks(4001) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 22, 16, 51, 51, 494, DateTimeKind.Local).AddTicks(4353), new DateTime(2022, 4, 22, 16, 51, 51, 494, DateTimeKind.Local).AddTicks(4356) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "ArticulosAlmacen",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2036), new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2046) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(1347), new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(1658) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2065), new DateTime(2022, 4, 19, 12, 39, 16, 819, DateTimeKind.Local).AddTicks(2068) });
        }
    }
}
