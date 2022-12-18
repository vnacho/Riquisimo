using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AccessInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessInvoices",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3773), new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3785) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(2297), new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(2875) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3804), new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3807) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessInvoices",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6572), new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6581) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(5478), new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6011) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6602), new DateTime(2020, 6, 9, 11, 45, 2, 686, DateTimeKind.Local).AddTicks(6606) });
        }
    }
}
