using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CampoPermisoControlAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PermisoControlAlmacen",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InventarioArticulosAlmacen",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 20, nullable: true),
                    FechaUltiActu = table.Column<DateTime>(nullable: false),
                    Unidades = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioArticulosAlmacen", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7528), new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7549) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(6825), new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7137) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7608), new DateTime(2022, 6, 9, 20, 0, 19, 819, DateTimeKind.Local).AddTicks(7611) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventarioArticulosAlmacen");

            migrationBuilder.DropColumn(
                name: "PermisoControlAlmacen",
                table: "Accounts");

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
    }
}
