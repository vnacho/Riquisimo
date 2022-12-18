using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class pedidocompras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompraPedidos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoPedido = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_CompraPedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompraPedidoLineas",
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
                    table.PrimaryKey("PK_CompraPedidoLineas", x => x.IdPedidoLinea);
                    table.ForeignKey(
                        name: "FK_CompraPedidoLineas_CompraPedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "CompraPedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2719), new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2729) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(1613), new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2150) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2750), new DateTime(2020, 12, 26, 13, 57, 24, 929, DateTimeKind.Local).AddTicks(2753) });

            migrationBuilder.CreateIndex(
                name: "IX_CompraPedidoLineas_PedidoId",
                table: "CompraPedidoLineas",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraPedidoLineas");

            migrationBuilder.DropTable(
                name: "CompraPedidos");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3773), new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3785) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(2297), new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(2875) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3804), new DateTime(2020, 9, 22, 8, 21, 14, 712, DateTimeKind.Local).AddTicks(3807) });
        }
    }
}
