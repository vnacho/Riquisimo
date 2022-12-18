using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class EsObra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoPostalObra",
                table: "Congresses",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionObra",
                table: "Congresses",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailDocumentacion",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailFacturacion",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "Congresses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Finalizada",
                table: "Congresses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "Congresses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PoblacionObra",
                table: "Congresses",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinciaObra",
                table: "Congresses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContratoObras",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    CongressID = table.Column<Guid>(nullable: false),
                    CodigoContrato = table.Column<string>(maxLength: 20, nullable: false),
                    FechaContratoInicio = table.Column<DateTime>(nullable: false),
                    ImporteContrato = table.Column<decimal>(nullable: false),
                    FechaContratoFin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoObras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoObras_Congresses_CongressID",
                        column: x => x.CongressID,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoObras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FicheroUrl = table.Column<string>(nullable: true),
                    FicheroNombre = table.Column<string>(nullable: true),
                    CongressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoObras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoObras_Congresses_CongressId",
                        column: x => x.CongressId,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoContratoObras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FicheroUrl = table.Column<string>(nullable: true),
                    FicheroNombre = table.Column<string>(nullable: true),
                    ContratoObraId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoContratoObras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoContratoObras_ContratoObras_ContratoObraId",
                        column: x => x.ContratoObraId,
                        principalTable: "ContratoObras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5964), new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5973) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5372), new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5658) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5990), new DateTime(2022, 4, 9, 21, 47, 33, 85, DateTimeKind.Local).AddTicks(5993) });

            migrationBuilder.CreateIndex(
                name: "IX_ContratoObras_CongressID",
                table: "ContratoObras",
                column: "CongressID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoContratoObras_ContratoObraId",
                table: "DocumentoContratoObras",
                column: "ContratoObraId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoObras_CongressId",
                table: "DocumentoObras",
                column: "CongressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoContratoObras");

            migrationBuilder.DropTable(
                name: "DocumentoObras");

            migrationBuilder.DropTable(
                name: "ContratoObras");

            migrationBuilder.DropColumn(
                name: "CodigoPostalObra",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "DireccionObra",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "EmailDocumentacion",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "EmailFacturacion",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Finalizada",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "PoblacionObra",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "ProvinciaObra",
                table: "Congresses");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6204), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6219) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(5298), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(5742) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6247), new DateTime(2022, 3, 23, 19, 49, 14, 254, DateTimeKind.Local).AddTicks(6252) });
        }
    }
}
