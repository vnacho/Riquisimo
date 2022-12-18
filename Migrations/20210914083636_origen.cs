using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class origen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioHora",
                table: "Personal",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Origen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivelAnalitico1 = table.Column<string>(maxLength: 3, nullable: false),
                    NivelAnalitico2 = table.Column<string>(maxLength: 5, nullable: false),
                    TipoCentroCosteId = table.Column<int>(nullable: false),
                    Anio = table.Column<int>(nullable: false),
                    Mes = table.Column<int>(nullable: false),
                    Ingresos = table.Column<decimal>(nullable: false),
                    Gastos = table.Column<decimal>(nullable: false),
                    FechaInforme = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Origen_TiposCentroCoste_TipoCentroCosteId",
                        column: x => x.TipoCentroCosteId,
                        principalTable: "TiposCentroCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(390), new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(400) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 36, 36, 45, DateTimeKind.Local).AddTicks(9556), new DateTime(2021, 9, 14, 10, 36, 36, 45, DateTimeKind.Local).AddTicks(9958) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(423), new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(427) });

            migrationBuilder.CreateIndex(
                name: "IX_Origen_TipoCentroCosteId",
                table: "Origen",
                column: "TipoCentroCosteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Origen");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioHora",
                table: "Personal",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6052), new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6065) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(4835), new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(5423) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6087), new DateTime(2021, 7, 12, 14, 33, 29, 122, DateTimeKind.Local).AddTicks(6091) });
        }
    }
}
