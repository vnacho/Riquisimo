using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AñadirMovimientosArticulosAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovimientosArticulosAlmacens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    ArticulosAlmacenId = table.Column<Guid>(nullable: false),
                    FechaMovimiento = table.Column<DateTime>(nullable: false),
                    movimiento = table.Column<string>(maxLength: 1, nullable: true),
                    Unidades = table.Column<decimal>(nullable: false),
                    CentroCosteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosArticulosAlmacens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosArticulosAlmacens_ArticulosAlmacen_ArticulosAlmacenId",
                        column: x => x.ArticulosAlmacenId,
                        principalTable: "ArticulosAlmacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientosArticulosAlmacens_CentrosCoste_CentroCosteId",
                        column: x => x.CentroCosteId,
                        principalTable: "CentrosCoste",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 11, 13, 19, 26, 780, DateTimeKind.Local).AddTicks(4321), new DateTime(2022, 6, 11, 13, 19, 26, 780, DateTimeKind.Local).AddTicks(4329) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 11, 13, 19, 26, 780, DateTimeKind.Local).AddTicks(3706), new DateTime(2022, 6, 11, 13, 19, 26, 780, DateTimeKind.Local).AddTicks(4004) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 11, 13, 19, 26, 780, DateTimeKind.Local).AddTicks(4347), new DateTime(2022, 6, 11, 13, 19, 26, 780, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosArticulosAlmacens_ArticulosAlmacenId",
                table: "MovimientosArticulosAlmacens",
                column: "ArticulosAlmacenId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosArticulosAlmacens_CentroCosteId",
                table: "MovimientosArticulosAlmacens",
                column: "CentroCosteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosArticulosAlmacens");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9121), new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9131) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(8368), new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(8801) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9149), new DateTime(2022, 6, 10, 21, 40, 30, 785, DateTimeKind.Local).AddTicks(9152) });
        }
    }
}
