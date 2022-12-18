using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ActualizacionCamposInventario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "InventarioArticulosAlmacen");

            migrationBuilder.AddColumn<Guid>(
                name: "ArticulosAlmacenId",
                table: "InventarioArticulosAlmacen",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9121), new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9131) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(8368), new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(8801) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9149), new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9152) });

            migrationBuilder.CreateIndex(
                name: "IX_InventarioArticulosAlmacen_ArticulosAlmacenId",
                table: "InventarioArticulosAlmacen",
                column: "ArticulosAlmacenId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventarioArticulosAlmacen_ArticulosAlmacen_ArticulosAlmacenId",
                table: "InventarioArticulosAlmacen",
                column: "ArticulosAlmacenId",
                principalTable: "ArticulosAlmacen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventarioArticulosAlmacen_ArticulosAlmacen_ArticulosAlmacenId",
                table: "InventarioArticulosAlmacen");

            migrationBuilder.DropIndex(
                name: "IX_InventarioArticulosAlmacen_ArticulosAlmacenId",
                table: "InventarioArticulosAlmacen");

            migrationBuilder.DropColumn(
                name: "ArticulosAlmacenId",
                table: "InventarioArticulosAlmacen");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "InventarioArticulosAlmacen",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7528), new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7549) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(6825), new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7137) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7608), new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7611) });
        }
    }
}
