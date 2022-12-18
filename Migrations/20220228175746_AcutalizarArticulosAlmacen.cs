using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AcutalizarArticulosAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "PruebaQuitar",
            //    table: "VentaPedidos");

            migrationBuilder.AddColumn<int>(
                name: "CentroCosteId",
                table: "ArticulosAlmacen",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ArticulosAlmacen",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Rate",
                table: "ArticulosAlmacen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rate2",
                table: "ArticulosAlmacen",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3227), new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3237) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(2620), new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(2917) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3255), new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3258) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CentroCosteId",
                table: "ArticulosAlmacen");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ArticulosAlmacen");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "ArticulosAlmacen");

            migrationBuilder.DropColumn(
                name: "Rate2",
                table: "ArticulosAlmacen");

            //migrationBuilder.AddColumn<string>(
            //    name: "PruebaQuitar",
            //    table: "VentaPedidos",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1114), new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1123) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(516), new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(808) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1137), new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1139) });
        }
    }
}
