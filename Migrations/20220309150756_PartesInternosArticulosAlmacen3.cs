using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class PartesInternosArticulosAlmacen3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartesInternosAlmacen_ArticulosAlmacen_ArticulosAlmacenId1",
                table: "PartesInternosAlmacen");

            migrationBuilder.DropIndex(
                name: "IX_PartesInternosAlmacen_ArticulosAlmacenId1",
                table: "PartesInternosAlmacen");

            migrationBuilder.DropColumn(
                name: "ArticulosAlmacenId",
                table: "PartesInternosAlmacen");

            migrationBuilder.DropColumn(
                name: "ArticulosAlmacenId1",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticulosAlmacenId",
                table: "PartesInternosAlmacen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ArticulosAlmacenId1",
                table: "PartesInternosAlmacen",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6318), new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6327) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(5680), new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(5987) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6362), new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6364) });

            migrationBuilder.CreateIndex(
                name: "IX_PartesInternosAlmacen_ArticulosAlmacenId1",
                table: "PartesInternosAlmacen",
                column: "ArticulosAlmacenId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PartesInternosAlmacen_ArticulosAlmacen_ArticulosAlmacenId1",
                table: "PartesInternosAlmacen",
                column: "ArticulosAlmacenId1",
                principalTable: "ArticulosAlmacen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
