using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class retencionBien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Retencion_Porcentaje",
                table: "CompraFacturaLineas");

            migrationBuilder.DropColumn(
                name: "TieneRetencion",
                table: "CompraFacturaLineas");

            migrationBuilder.AddColumn<int>(
                name: "Retencion_Porcentaje",
                table: "CompraFacturas",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneRetencion",
                table: "CompraFacturas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3521), new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3533) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(1936), new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(2799) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3556), new DateTime(2021, 1, 12, 17, 18, 32, 666, DateTimeKind.Local).AddTicks(3559) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Retencion_Porcentaje",
                table: "CompraFacturas");

            migrationBuilder.DropColumn(
                name: "TieneRetencion",
                table: "CompraFacturas");

            migrationBuilder.AddColumn<int>(
                name: "Retencion_Porcentaje",
                table: "CompraFacturaLineas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneRetencion",
                table: "CompraFacturaLineas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 11, 18, 56, 28, 971, DateTimeKind.Local).AddTicks(8047), new DateTime(2021, 1, 11, 18, 56, 28, 971, DateTimeKind.Local).AddTicks(8057) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 11, 18, 56, 28, 971, DateTimeKind.Local).AddTicks(6730), new DateTime(2021, 1, 11, 18, 56, 28, 971, DateTimeKind.Local).AddTicks(7289) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 11, 18, 56, 28, 971, DateTimeKind.Local).AddTicks(8184), new DateTime(2021, 1, 11, 18, 56, 28, 971, DateTimeKind.Local).AddTicks(8187) });
        }
    }
}
