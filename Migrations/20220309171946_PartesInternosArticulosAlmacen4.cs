using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class PartesInternosArticulosAlmacen4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArticulosAlmacenId",
                table: "PartesInternosAlmacen",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 18, 19, 45, 906, DateTimeKind.Local).AddTicks(5688), new DateTime(2022, 3, 9, 18, 19, 45, 906, DateTimeKind.Local).AddTicks(5696) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 18, 19, 45, 906, DateTimeKind.Local).AddTicks(5090), new DateTime(2022, 3, 9, 18, 19, 45, 906, DateTimeKind.Local).AddTicks(5381) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 18, 19, 45, 906, DateTimeKind.Local).AddTicks(5713), new DateTime(2022, 3, 9, 18, 19, 45, 906, DateTimeKind.Local).AddTicks(5716) });

            migrationBuilder.CreateIndex(
                name: "IX_PartesInternosAlmacen_ArticulosAlmacenId",
                table: "PartesInternosAlmacen",
                column: "ArticulosAlmacenId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartesInternosAlmacen_ArticulosAlmacen_ArticulosAlmacenId",
                table: "PartesInternosAlmacen",
                column: "ArticulosAlmacenId",
                principalTable: "ArticulosAlmacen",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartesInternosAlmacen_ArticulosAlmacen_ArticulosAlmacenId",
                table: "PartesInternosAlmacen");

            migrationBuilder.DropIndex(
                name: "IX_PartesInternosAlmacen_ArticulosAlmacenId",
                table: "PartesInternosAlmacen");

            migrationBuilder.DropColumn(
                name: "ArticulosAlmacenId",
                table: "PartesInternosAlmacen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 16, 7, 56, 445, DateTimeKind.Local).AddTicks(4278), new DateTime(2022, 3, 9, 16, 7, 56, 445, DateTimeKind.Local).AddTicks(4286) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 16, 7, 56, 445, DateTimeKind.Local).AddTicks(3685), new DateTime(2022, 3, 9, 16, 7, 56, 445, DateTimeKind.Local).AddTicks(3973) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 16, 7, 56, 445, DateTimeKind.Local).AddTicks(4303), new DateTime(2022, 3, 9, 16, 7, 56, 445, DateTimeKind.Local).AddTicks(4305) });
        }
    }
}
