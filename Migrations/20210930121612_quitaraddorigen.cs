using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class quitaraddorigen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddOrigen",
                table: "Origen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5297), new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5306) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(4129), new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(4698) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5326), new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5329) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddOrigen",
                table: "Origen",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3014), new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3023) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(2215), new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(2587) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3045), new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3048) });
        }
    }
}
