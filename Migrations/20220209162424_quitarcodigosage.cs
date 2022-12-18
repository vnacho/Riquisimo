using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class quitarcodigosage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoSage",
                table: "VentaFacturas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8569), new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8577) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(7965), new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8257) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8593), new DateTime(2022, 2, 9, 17, 24, 24, 358, DateTimeKind.Local).AddTicks(8595) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodigoSage",
                table: "VentaFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4151), new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4159) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(3502), new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(3813) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4176), new DateTime(2022, 2, 3, 11, 42, 4, 503, DateTimeKind.Local).AddTicks(4179) });
        }
    }
}
