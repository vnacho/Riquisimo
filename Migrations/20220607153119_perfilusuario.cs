using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class perfilusuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PerfilUsuario",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8497), new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8506) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(7871), new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8170) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8524), new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8527) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerfilUsuario",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1155), new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1163) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(517), new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(823) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1179), new DateTime(2022, 5, 27, 15, 0, 40, 979, DateTimeKind.Local).AddTicks(1182) });
        }
    }
}
