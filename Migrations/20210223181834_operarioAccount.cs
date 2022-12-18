using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class operarioAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Operario",
                table: "Accounts",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8861), new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8872) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(7500), new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8096) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8915), new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8918) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operario",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7761), new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(6549), new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7112) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7795), new DateTime(2021, 2, 8, 16, 55, 49, 502, DateTimeKind.Local).AddTicks(7798) });
        }
    }
}
