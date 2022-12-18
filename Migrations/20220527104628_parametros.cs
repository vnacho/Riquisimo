using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class parametros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parametros",
                columns: table => new
                {
                    Codigo = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametros", x => x.Codigo);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4482), new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4493) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(3797), new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4147) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4511), new DateTime(2022, 5, 27, 12, 46, 27, 625, DateTimeKind.Local).AddTicks(4514) });

            migrationBuilder.InsertData(
                table: "Parametros",
                columns: new[] { "Codigo", "Descripcion", "Valor" },
                values: new object[] { "FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA", "Indica la fecha límite inferior bajo la que no se permitirá introducir facturas de compra o venta (Fecha de factura). Si se deja vacía no se producirá tal restricción.", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parametros");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3197), new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3205) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(2571), new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3223), new DateTime(2022, 5, 4, 13, 47, 45, 885, DateTimeKind.Local).AddTicks(3226) });
        }
    }
}
