using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class sociedadcientifica : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SociedadCientificaId",
                table: "Congresses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CargosJuntaDirectivaSociedadCientifica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargosJuntaDirectivaSociedadCientifica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SociedadesCientificas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SociedadesCientificas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SociosSociedadCientifica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SociedadCientificaId = table.Column<int>(nullable: false),
                    CargoJuntaDirectivaSociedadCientificaId = table.Column<int>(nullable: true),
                    NIF = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Apellidos = table.Column<string>(nullable: false),
                    JuntaDirectiva = table.Column<bool>(nullable: false),
                    FechaInicioCargo = table.Column<DateTime>(nullable: false),
                    FechaFinCargo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SociosSociedadCientifica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SociosSociedadCientifica_CargosJuntaDirectivaSociedadCientifica_CargoJuntaDirectivaSociedadCientificaId",
                        column: x => x.CargoJuntaDirectivaSociedadCientificaId,
                        principalTable: "CargosJuntaDirectivaSociedadCientifica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SociosSociedadCientifica_SociedadesCientificas_SociedadCientificaId",
                        column: x => x.SociedadCientificaId,
                        principalTable: "SociedadesCientificas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9239), new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(8614), new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(8915) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9270), new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9272) });

            migrationBuilder.CreateIndex(
                name: "IX_Congresses_SociedadCientificaId",
                table: "Congresses",
                column: "SociedadCientificaId");

            migrationBuilder.CreateIndex(
                name: "IX_SociosSociedadCientifica_CargoJuntaDirectivaSociedadCientificaId",
                table: "SociosSociedadCientifica",
                column: "CargoJuntaDirectivaSociedadCientificaId");

            migrationBuilder.CreateIndex(
                name: "IX_SociosSociedadCientifica_SociedadCientificaId",
                table: "SociosSociedadCientifica",
                column: "SociedadCientificaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Congresses_SociedadesCientificas_SociedadCientificaId",
                table: "Congresses",
                column: "SociedadCientificaId",
                principalTable: "SociedadesCientificas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Congresses_SociedadesCientificas_SociedadCientificaId",
                table: "Congresses");

            migrationBuilder.DropTable(
                name: "SociosSociedadCientifica");

            migrationBuilder.DropTable(
                name: "CargosJuntaDirectivaSociedadCientifica");

            migrationBuilder.DropTable(
                name: "SociedadesCientificas");

            migrationBuilder.DropIndex(
                name: "IX_Congresses_SociedadCientificaId",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "SociedadCientificaId",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 25, 15, 56, 51, 836, DateTimeKind.Local).AddTicks(9178), new DateTime(2022, 9, 25, 15, 56, 51, 836, DateTimeKind.Local).AddTicks(9186) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 25, 15, 56, 51, 836, DateTimeKind.Local).AddTicks(8556), new DateTime(2022, 9, 25, 15, 56, 51, 836, DateTimeKind.Local).AddTicks(8857) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 25, 15, 56, 51, 836, DateTimeKind.Local).AddTicks(9202), new DateTime(2022, 9, 25, 15, 56, 51, 836, DateTimeKind.Local).AddTicks(9205) });
        }
    }
}
