using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class asistente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asistente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIF = table.Column<string>(maxLength: 20, nullable: false),
                    Tratamiento = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 80, nullable: false),
                    Apellidos = table.Column<string>(maxLength: 80, nullable: true),
                    CentroTrabajo = table.Column<string>(maxLength: 80, nullable: true),
                    CategoriaProfesional = table.Column<string>(maxLength: 80, nullable: true),
                    Cargo = table.Column<string>(maxLength: 80, nullable: true),
                    FechaActualizacionCargo = table.Column<DateTime>(nullable: false),
                    Direccion = table.Column<string>(maxLength: 80, nullable: true),
                    Poblacion = table.Column<string>(maxLength: 80, nullable: true),
                    Ciudad = table.Column<string>(maxLength: 80, nullable: true),
                    Telefono1 = table.Column<string>(maxLength: 15, nullable: true),
                    Telefono2 = table.Column<string>(maxLength: 15, nullable: true),
                    Email1 = table.Column<string>(maxLength: 80, nullable: true),
                    Email2 = table.Column<string>(maxLength: 80, nullable: true),
                    Asociacion = table.Column<string>(maxLength: 80, nullable: true),
                    CodigoPais = table.Column<string>(nullable: true),
                    NombrePais = table.Column<string>(nullable: true),
                    CodigoPostal = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistente", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4541), new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4551) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(3719), new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4109) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4571), new DateTime(2021, 9, 30, 11, 46, 19, 129, DateTimeKind.Local).AddTicks(4575) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistente");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 10, 23, 13, 683, DateTimeKind.Local).AddTicks(8775), new DateTime(2021, 9, 30, 10, 23, 13, 683, DateTimeKind.Local).AddTicks(8786) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 10, 23, 13, 683, DateTimeKind.Local).AddTicks(7731), new DateTime(2021, 9, 30, 10, 23, 13, 683, DateTimeKind.Local).AddTicks(8194) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 10, 23, 13, 683, DateTimeKind.Local).AddTicks(8809), new DateTime(2021, 9, 30, 10, 23, 13, 683, DateTimeKind.Local).AddTicks(8812) });
        }
    }
}
