using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class añadiriconotienda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaTikect",
                table: "Tikects",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icono",
                table: "Tiendas",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 15, 17, 0, 32, 823, DateTimeKind.Local).AddTicks(9895), new DateTime(2022, 12, 15, 17, 0, 32, 823, DateTimeKind.Local).AddTicks(9905) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 15, 17, 0, 32, 823, DateTimeKind.Local).AddTicks(9251), new DateTime(2022, 12, 15, 17, 0, 32, 823, DateTimeKind.Local).AddTicks(9567) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 15, 17, 0, 32, 823, DateTimeKind.Local).AddTicks(9924), new DateTime(2022, 12, 15, 17, 0, 32, 823, DateTimeKind.Local).AddTicks(9926) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icono",
                table: "Tiendas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaTikect",
                table: "Tikects",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7489), new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7500) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(6827), new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7147) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7517), new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7519) });
        }
    }
}
