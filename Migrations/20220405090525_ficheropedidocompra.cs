using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ficheropedidocompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FicheroNombre",
                table: "CompraPedidos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FicheroUrl",
                table: "CompraPedidos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9392), new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9404) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(8564), new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(8966) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9432), new DateTime(2022, 4, 5, 11, 5, 25, 164, DateTimeKind.Local).AddTicks(9436) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FicheroNombre",
                table: "CompraPedidos");

            migrationBuilder.DropColumn(
                name: "FicheroUrl",
                table: "CompraPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1760), new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1770) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1083), new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1412) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1789), new DateTime(2022, 4, 1, 11, 52, 9, 751, DateTimeKind.Local).AddTicks(1792) });
        }
    }
}
