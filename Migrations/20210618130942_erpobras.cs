using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class erpobras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estructura",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PorcentajeReparto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CentroCosteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estructura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estructura_CentrosCoste_CentroCosteId",
                        column: x => x.CentroCosteId,
                        principalTable: "CentrosCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrabacionE9",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(nullable: false),
                    EntradaSalida = table.Column<string>(maxLength: 1, nullable: false),
                    Importe = table.Column<decimal>(nullable: false),
                    CentroCosteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrabacionE9", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrabacionE9_CentrosCoste_CentroCosteId",
                        column: x => x.CentroCosteId,
                        principalTable: "CentrosCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiposTarifa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(maxLength: 1, nullable: false),
                    Nombre = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTarifa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    NIF = table.Column<string>(nullable: false),
                    Categoria = table.Column<string>(maxLength: 40, nullable: false),
                    CosteEstandar = table.Column<decimal>(type: "decimal(13,2)", nullable: true),
                    FechaAlta = table.Column<DateTime>(nullable: true),
                    FechaUltimaRevisionMedica = table.Column<DateTime>(nullable: true),
                    FechaBaja = table.Column<DateTime>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    TipoTarifaId = table.Column<int>(nullable: false),
                    CentroCosteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personal_CentrosCoste_CentroCosteId",
                        column: x => x.CentroCosteId,
                        principalTable: "CentrosCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personal_TiposTarifa_TipoTarifaId",
                        column: x => x.TipoTarifaId,
                        principalTable: "TiposTarifa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartePersonal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoParte = table.Column<string>(maxLength: 1, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Unidades = table.Column<decimal>(nullable: false),
                    Precio = table.Column<decimal>(nullable: false),
                    Importe = table.Column<decimal>(nullable: false),
                    PersonalId = table.Column<int>(nullable: false),
                    CentroCosteId = table.Column<int>(nullable: false),
                    PersonalId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartePersonal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartePersonal_CentrosCoste_CentroCosteId",
                        column: x => x.CentroCosteId,
                        principalTable: "CentrosCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartePersonal_Personal_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartePersonal_Personal_PersonalId1",
                        column: x => x.PersonalId1,
                        principalTable: "Personal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6711), new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6725) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(5505), new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6091) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6748), new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6751) });

            migrationBuilder.CreateIndex(
                name: "IX_Estructura_CentroCosteId",
                table: "Estructura",
                column: "CentroCosteId");

            migrationBuilder.CreateIndex(
                name: "IX_GrabacionE9_CentroCosteId",
                table: "GrabacionE9",
                column: "CentroCosteId");

            migrationBuilder.CreateIndex(
                name: "IX_PartePersonal_CentroCosteId",
                table: "PartePersonal",
                column: "CentroCosteId");

            migrationBuilder.CreateIndex(
                name: "IX_PartePersonal_PersonalId",
                table: "PartePersonal",
                column: "PersonalId");

            migrationBuilder.CreateIndex(
                name: "IX_PartePersonal_PersonalId1",
                table: "PartePersonal",
                column: "PersonalId1");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_CentroCosteId",
                table: "Personal",
                column: "CentroCosteId");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_TipoTarifaId",
                table: "Personal",
                column: "TipoTarifaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estructura");

            migrationBuilder.DropTable(
                name: "GrabacionE9");

            migrationBuilder.DropTable(
                name: "PartePersonal");

            migrationBuilder.DropTable(
                name: "Personal");

            migrationBuilder.DropTable(
                name: "TiposTarifa");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8715), new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8725) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(7325), new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(7991) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8761), new DateTime(2021, 6, 15, 19, 1, 13, 397, DateTimeKind.Local).AddTicks(8765) });
        }
    }
}
