using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class encuentros_campos_extra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Libres",
                table: "Encuentros",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Reservados",
                table: "Encuentros",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalComensales",
                table: "Encuentros",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3368), new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3376) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(2750), new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3048) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3392), new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3395) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Libres",
                table: "Encuentros");

            migrationBuilder.DropColumn(
                name: "Reservados",
                table: "Encuentros");

            migrationBuilder.DropColumn(
                name: "TotalComensales",
                table: "Encuentros");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3500), new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3509) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(2863), new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3166) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3529), new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3532) });
        }
    }
}
