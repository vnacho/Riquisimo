using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class comites_puestos_tipos_restructurar_id2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PuestosComite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuestosComite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposComite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposComite", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1782), new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1804) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1016), new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1388) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1821), new DateTime(2022, 7, 19, 18, 26, 12, 803, DateTimeKind.Local).AddTicks(1824) });

            migrationBuilder.InsertData(
                table: "PuestosComite",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 16, "Presidenta de las Jornadas" },
                    { 15, "Vocales" },
                    { 14, "Coordinadora Área Científica" },
                    { 12, "Tesorero" },
                    { 11, "Secretaria" },
                    { 10, "Secretario" },
                    { 13, "Tesorera" },
                    { 8, "Vicepresidenta" },
                    { 7, "Vicepresidente" },
                    { 6, "Presidenta" },
                    { 5, "Presidente" },
                    { 4, "Miembros de Honor" },
                    { 3, "Vicepresidencia de Honor" },
                    { 2, "Presidencia de Honor" },
                    { 9, "Coordinadora" },
                    { 1, "" }
                });

            migrationBuilder.InsertData(
                table: "TiposComite",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "" },
                    { 2, "Junta Directiva" },
                    { 3, "Comité de Honor" },
                    { 4, "Comité Organizador" },
                    { 5, "Comité Científico" },
                    { 6, "Comité Asesor" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PuestosComite");

            migrationBuilder.DropTable(
                name: "TiposComite");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 22, 2, 709, DateTimeKind.Local).AddTicks(210), new DateTime(2022, 7, 19, 18, 22, 2, 709, DateTimeKind.Local).AddTicks(220) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 22, 2, 708, DateTimeKind.Local).AddTicks(9460), new DateTime(2022, 7, 19, 18, 22, 2, 708, DateTimeKind.Local).AddTicks(9784) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 18, 22, 2, 709, DateTimeKind.Local).AddTicks(239), new DateTime(2022, 7, 19, 18, 22, 2, 709, DateTimeKind.Local).AddTicks(241) });
        }
    }
}
