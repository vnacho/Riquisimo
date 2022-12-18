using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class pruebasconjv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PruebaQuitar",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 13, 2, 37, 513, DateTimeKind.Local).AddTicks(7927), new DateTime(2022, 2, 17, 13, 2, 37, 513, DateTimeKind.Local).AddTicks(7942) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 13, 2, 37, 513, DateTimeKind.Local).AddTicks(6947), new DateTime(2022, 2, 17, 13, 2, 37, 513, DateTimeKind.Local).AddTicks(7391) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 13, 2, 37, 513, DateTimeKind.Local).AddTicks(7973), new DateTime(2022, 2, 17, 13, 2, 37, 513, DateTimeKind.Local).AddTicks(7977) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PruebaQuitar",
                table: "VentaPedidos");

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
