using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class reservamesa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReservaMesa",
                table: "Encuentros",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5450), new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5460) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(4809), new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5115) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5479), new DateTime(2022, 10, 19, 12, 18, 6, 197, DateTimeKind.Local).AddTicks(5482) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservaMesa",
                table: "Encuentros");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(52), new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(62) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 15, 48, 154, DateTimeKind.Local).AddTicks(9377), new DateTime(2022, 10, 11, 15, 15, 48, 154, DateTimeKind.Local).AddTicks(9701) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(82), new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(84) });
        }
    }
}
