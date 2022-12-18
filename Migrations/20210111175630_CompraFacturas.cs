using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CompraFacturas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompraFacturaId",
                table: "CompraAlbaranes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GeneradoAutomaticamente",
                table: "CompraAlbaranes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CompraFacturas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoSage = table.Column<string>(nullable: true),
                    NumeroFactura = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    CodigoProveedor = table.Column<string>(nullable: false),
                    NombreProveedor = table.Column<string>(nullable: false),
                    CodigoOperario = table.Column<string>(nullable: false),
                    NombreOperario = table.Column<string>(nullable: false),
                    EstadoFactura = table.Column<int>(nullable: false),
                    TieneFichero = table.Column<bool>(nullable: false),
                    Fichero = table.Column<byte[]>(nullable: true),
                    FicheroUrl = table.Column<string>(nullable: true),
                    FicheroNombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraFacturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompraFacturaLineas",
                columns: table => new
                {
                    IdFacturaLinea = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraFacturaId = table.Column<int>(nullable: false),
                    CodigoArticulo = table.Column<string>(nullable: false),
                    NombreArticulo = table.Column<string>(nullable: true),
                    ObservacionesFacturaLinea = table.Column<string>(nullable: true),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    Unidades = table.Column<decimal>(nullable: false),
                    BaseImponiblePrecioUnitario = table.Column<decimal>(nullable: false),
                    BaseImponibleTotal = table.Column<decimal>(nullable: false),
                    IVA_Porcentaje = table.Column<int>(nullable: false),
                    TieneRetencion = table.Column<bool>(nullable: false),
                    Retencion_Porcentaje = table.Column<int>(nullable: true),
                    Orden = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraFacturaLineas", x => x.IdFacturaLinea);
                    table.ForeignKey(
                        name: "FK_CompraFacturaLineas_CompraFacturas_CompraFacturaId",
                        column: x => x.CompraFacturaId,
                        principalTable: "CompraFacturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CompraAlbaranes_CompraFacturaId",
                table: "CompraAlbaranes",
                column: "CompraFacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraFacturaLineas_CompraFacturaId",
                table: "CompraFacturaLineas",
                column: "CompraFacturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraAlbaranes_CompraFacturas_CompraFacturaId",
                table: "CompraAlbaranes",
                column: "CompraFacturaId",
                principalTable: "CompraFacturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraAlbaranes_CompraFacturas_CompraFacturaId",
                table: "CompraAlbaranes");

            migrationBuilder.DropTable(
                name: "CompraFacturaLineas");

            migrationBuilder.DropTable(
                name: "CompraFacturas");

            migrationBuilder.DropIndex(
                name: "IX_CompraAlbaranes_CompraFacturaId",
                table: "CompraAlbaranes");

            migrationBuilder.DropColumn(
                name: "CompraFacturaId",
                table: "CompraAlbaranes");

            migrationBuilder.DropColumn(
                name: "GeneradoAutomaticamente",
                table: "CompraAlbaranes");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2268), new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2281) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(642), new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(1552) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2306), new DateTime(2021, 1, 7, 13, 8, 46, 638, DateTimeKind.Local).AddTicks(2310) });
        }
    }
}
