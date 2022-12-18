using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class Ponentes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "Creditos",
            //    table: "Registrations",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HaAsistido",
            //    table: "Registrations",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CategoriaInscritoId",
            //    table: "Registrant",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "CategoriasInscritos",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(nullable: false),
            //        Nombre = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CategoriasInscritos", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "PuestosComite",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuestosComite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposComite",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposComite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ponentes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdExterno = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(nullable: false),
                    Apellidos = table.Column<string>(nullable: false),
                    NIF = table.Column<string>(nullable: false),
                    Localidad = table.Column<string>(nullable: true),
                    Provincia = table.Column<string>(nullable: true),
                    Cargo = table.Column<string>(nullable: true),
                    CentroTrabajo = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    Movil = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    Mail2 = table.Column<string>(nullable: true),
                    Tratamiento = table.Column<int>(nullable: true),
                    AmbitoComite = table.Column<int>(nullable: true),
                    Activo = table.Column<bool>(nullable: false),
                    Visible = table.Column<bool>(nullable: false),
                    UltimoAcceso = table.Column<DateTime>(nullable: true),
                    Comentarios = table.Column<string>(nullable: true),
                    Superevaluador = table.Column<bool>(nullable: false),
                    Visualizador = table.Column<bool>(nullable: false),
                    JuntaDirectiva = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    TipoComiteId = table.Column<string>(nullable: true),
                    PuestoComiteId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ponentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ponentes_PuestosComite_PuestoComiteId",
                        column: x => x.PuestoComiteId,
                        principalTable: "PuestosComite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ponentes_TiposComite_TipoComiteId",
                        column: x => x.TipoComiteId,
                        principalTable: "TiposComite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.InsertData(
            //    table: "CategoriasInscritos",
            //    columns: new[] { "Id", "Nombre" },
            //    values: new object[,]
            //    {
            //        { "1", "Asistente" },
            //        { "2", "Ponente" },
            //        { "3", "Moderador" },
            //        { "4", "Comité Científico" },
            //        { "5", "Comité Organizador" },
            //        { "6", "Junta Directiva" }
            //    });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4313), new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4321) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(3618), new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4340), new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4342) });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Registrant_CategoriaInscritoId",
            //    table: "Registrant",
            //    column: "CategoriaInscritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponentes_PuestoComiteId",
                table: "Ponentes",
                column: "PuestoComiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponentes_TipoComiteId",
                table: "Ponentes",
                column: "TipoComiteId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Registrant_CategoriasInscritos_CategoriaInscritoId",
            //    table: "Registrant",
            //    column: "CategoriaInscritoId",
            //    principalTable: "CategoriasInscritos",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrant_CategoriasInscritos_CategoriaInscritoId",
                table: "Registrant");

            migrationBuilder.DropTable(
                name: "CategoriasInscritos");

            migrationBuilder.DropTable(
                name: "Ponentes");

            migrationBuilder.DropTable(
                name: "PuestosComite");

            migrationBuilder.DropTable(
                name: "TiposComite");

            migrationBuilder.DropIndex(
                name: "IX_Registrant_CategoriaInscritoId",
                table: "Registrant");

            migrationBuilder.DropColumn(
                name: "Creditos",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "HaAsistido",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "CategoriaInscritoId",
                table: "Registrant");

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
        }
    }
}
