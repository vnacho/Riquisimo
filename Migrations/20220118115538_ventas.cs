using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VentaFacturas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Registro = table.Column<string>(maxLength: 6, nullable: true),
                    CodigoSage = table.Column<int>(nullable: false),
                    NumeroFactura = table.Column<string>(maxLength: 24, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    BaseImponible = table.Column<decimal>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    CodigoCliente = table.Column<string>(nullable: false),
                    NombreCliente = table.Column<string>(nullable: false),
                    CodigoOperario = table.Column<string>(nullable: false),
                    NombreOperario = table.Column<string>(nullable: false),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    EstadoFactura = table.Column<int>(nullable: false),
                    TieneRetencion = table.Column<bool>(nullable: false),
                    Retencion_Porcentaje = table.Column<decimal>(nullable: true),
                    Pagada = table.Column<bool>(nullable: false),
                    FicheroUrl = table.Column<string>(nullable: true),
                    FicheroNombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaFacturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentaPedidos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoPedido = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    CodigoCliente = table.Column<string>(nullable: false),
                    NombreCliente = table.Column<string>(nullable: false),
                    CodigoOperario = table.Column<string>(nullable: false),
                    NombreOperario = table.Column<string>(nullable: false),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    EstadoPedido = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaPedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentaAlbaranes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoAlbaran = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    CodigoCliente = table.Column<string>(nullable: false),
                    NombreCliente = table.Column<string>(nullable: false),
                    CodigoOperario = table.Column<string>(nullable: false),
                    NombreOperario = table.Column<string>(nullable: false),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    EstadoAlbaran = table.Column<int>(nullable: false),
                    GeneradoAutomaticamente = table.Column<bool>(nullable: false),
                    VentaFacturaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaAlbaranes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaAlbaranes_VentaFacturas_VentaFacturaId",
                        column: x => x.VentaFacturaId,
                        principalTable: "VentaFacturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VentaPedidoLineas",
                columns: table => new
                {
                    IdPedidoLinea = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(nullable: false),
                    CodigoArticulo = table.Column<string>(nullable: false),
                    NombreArticulo = table.Column<string>(nullable: true),
                    ObservacionesPedidoLinea = table.Column<string>(nullable: true),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    Unidades = table.Column<decimal>(nullable: false),
                    UnidadesPendientes = table.Column<decimal>(nullable: false),
                    PrecioUnitario = table.Column<decimal>(nullable: false),
                    TotalPedidoLinea = table.Column<decimal>(nullable: false),
                    Orden = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaPedidoLineas", x => x.IdPedidoLinea);
                    table.ForeignKey(
                        name: "FK_VentaPedidoLineas_VentaPedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "VentaPedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentaAlbaranLineas",
                columns: table => new
                {
                    IdAlbaranLinea = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbaranId = table.Column<int>(nullable: false),
                    VentaPedidoLineaId = table.Column<int>(nullable: true),
                    CodigoArticulo = table.Column<string>(nullable: false),
                    NombreArticulo = table.Column<string>(nullable: true),
                    ObservacionesAlbaranLinea = table.Column<string>(nullable: true),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    Unidades = table.Column<decimal>(nullable: false),
                    PrecioUnitario = table.Column<decimal>(nullable: false),
                    TotalAlbaranLinea = table.Column<decimal>(nullable: false),
                    Orden = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaAlbaranLineas", x => x.IdAlbaranLinea);
                    table.ForeignKey(
                        name: "FK_VentaAlbaranLineas_VentaAlbaranes_AlbaranId",
                        column: x => x.AlbaranId,
                        principalTable: "VentaAlbaranes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VentaAlbaranLineas_VentaPedidoLineas_VentaPedidoLineaId",
                        column: x => x.VentaPedidoLineaId,
                        principalTable: "VentaPedidoLineas",
                        principalColumn: "IdPedidoLinea",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VentaFacturaLineas",
                columns: table => new
                {
                    IdFacturaLinea = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaFacturaId = table.Column<int>(nullable: false),
                    CodigoArticulo = table.Column<string>(nullable: false),
                    NombreArticulo = table.Column<string>(nullable: true),
                    ObservacionesFacturaLinea = table.Column<string>(nullable: true),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    Unidades = table.Column<decimal>(nullable: false),
                    BaseImponiblePrecioUnitario = table.Column<decimal>(nullable: false),
                    BaseImponibleTotal = table.Column<decimal>(nullable: false),
                    IVA_Porcentaje = table.Column<int>(nullable: true),
                    CodigoTipoIVA = table.Column<string>(nullable: false),
                    Orden = table.Column<int>(nullable: false),
                    VentaAlbaranLineaId = table.Column<int>(nullable: true),
                    VentaPedidoLineaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaFacturaLineas", x => x.IdFacturaLinea);
                    table.ForeignKey(
                        name: "FK_VentaFacturaLineas_VentaAlbaranLineas_VentaAlbaranLineaId",
                        column: x => x.VentaAlbaranLineaId,
                        principalTable: "VentaAlbaranLineas",
                        principalColumn: "IdAlbaranLinea",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VentaFacturaLineas_VentaFacturas_VentaFacturaId",
                        column: x => x.VentaFacturaId,
                        principalTable: "VentaFacturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VentaFacturaLineas_VentaPedidoLineas_VentaPedidoLineaId",
                        column: x => x.VentaPedidoLineaId,
                        principalTable: "VentaPedidoLineas",
                        principalColumn: "IdPedidoLinea",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2273), new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2281) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(1622), new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(1941) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2297), new DateTime(2022, 1, 18, 12, 55, 37, 804, DateTimeKind.Local).AddTicks(2300) });

            migrationBuilder.CreateIndex(
                name: "IX_VentaAlbaranes_VentaFacturaId",
                table: "VentaAlbaranes",
                column: "VentaFacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaAlbaranLineas_AlbaranId",
                table: "VentaAlbaranLineas",
                column: "AlbaranId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaAlbaranLineas_VentaPedidoLineaId",
                table: "VentaAlbaranLineas",
                column: "VentaPedidoLineaId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaFacturaLineas_VentaAlbaranLineaId",
                table: "VentaFacturaLineas",
                column: "VentaAlbaranLineaId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaFacturaLineas_VentaFacturaId",
                table: "VentaFacturaLineas",
                column: "VentaFacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaFacturaLineas_VentaPedidoLineaId",
                table: "VentaFacturaLineas",
                column: "VentaPedidoLineaId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaPedidoLineas_PedidoId",
                table: "VentaPedidoLineas",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VentaFacturaLineas");

            migrationBuilder.DropTable(
                name: "VentaAlbaranLineas");

            migrationBuilder.DropTable(
                name: "VentaAlbaranes");

            migrationBuilder.DropTable(
                name: "VentaPedidoLineas");

            migrationBuilder.DropTable(
                name: "VentaFacturas");

            migrationBuilder.DropTable(
                name: "VentaPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3930), new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3939) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3316), new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3612) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3956), new DateTime(2022, 1, 13, 10, 58, 42, 877, DateTimeKind.Local).AddTicks(3959) });
        }
    }
}
