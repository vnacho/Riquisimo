using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class compraAlbaranes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompraAlbaranes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoAlbaran = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    CodigoProveedor = table.Column<string>(nullable: false),
                    NombreProveedor = table.Column<string>(nullable: false),
                    CodigoOperario = table.Column<string>(nullable: false),
                    NombreOperario = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraAlbaranes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompraAlbaranLineas",
                columns: table => new
                {
                    IdAlbaranLinea = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbaranId = table.Column<int>(nullable: false),
                    CodigoArticulo = table.Column<string>(nullable: false),
                    NombreArticulo = table.Column<string>(nullable: true),
                    ObservacionesAlbaranLinea = table.Column<string>(nullable: true),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    Unidades = table.Column<decimal>(nullable: false),
                    UnidadesPendientes = table.Column<decimal>(nullable: false),
                    PrecioUnitario = table.Column<decimal>(nullable: false),
                    TotalAlbaranLinea = table.Column<decimal>(nullable: false),
                    Orden = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraAlbaranLineas", x => x.IdAlbaranLinea);
                    table.ForeignKey(
                        name: "FK_CompraAlbaranLineas_CompraAlbaranes_AlbaranId",
                        column: x => x.AlbaranId,
                        principalTable: "CompraAlbaranes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 22, 8, 798, DateTimeKind.Local).AddTicks(9837), new DateTime(2021, 1, 7, 11, 22, 8, 798, DateTimeKind.Local).AddTicks(9848) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 22, 8, 798, DateTimeKind.Local).AddTicks(8685), new DateTime(2021, 1, 7, 11, 22, 8, 798, DateTimeKind.Local).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 11, 22, 8, 798, DateTimeKind.Local).AddTicks(9866), new DateTime(2021, 1, 7, 11, 22, 8, 798, DateTimeKind.Local).AddTicks(9870) });

            migrationBuilder.CreateIndex(
                name: "IX_CompraAlbaranLineas_AlbaranId",
                table: "CompraAlbaranLineas",
                column: "AlbaranId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraAlbaranLineas");

            migrationBuilder.DropTable(
                name: "CompraAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6367), new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6382) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(5203), new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(5767) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6402), new DateTime(2021, 1, 5, 18, 34, 2, 749, DateTimeKind.Local).AddTicks(6405) });
        }
    }
}
