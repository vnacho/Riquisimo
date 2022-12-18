using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class comites_puestos_tipos_nombre_no_requerido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "TiposComite",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "PuestosComite",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 17, 46, 34, 484, DateTimeKind.Local).AddTicks(3026), new DateTime(2022, 7, 19, 17, 46, 34, 484, DateTimeKind.Local).AddTicks(3052) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 17, 46, 34, 484, DateTimeKind.Local).AddTicks(1343), new DateTime(2022, 7, 19, 17, 46, 34, 484, DateTimeKind.Local).AddTicks(2151) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 17, 46, 34, 484, DateTimeKind.Local).AddTicks(3098), new DateTime(2022, 7, 19, 17, 46, 34, 484, DateTimeKind.Local).AddTicks(3105) });

            migrationBuilder.InsertData(
                table: "PuestosComite",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { "16", "Presidenta de las Jornadas" },
                    { "15", "Vocales" },
                    { "14", "Coordinadora Área Científica" },
                    { "12", "Tesorero" },
                    { "11", "Secretaria" },
                    { "10", "Secretario" },
                    { "13", "Tesorera" },
                    { "8", "Vicepresidenta" },
                    { "7", "Vicepresidente" },
                    { "6", "Presidenta" },
                    { "5", "Presidente" },
                    { "4", "Miembros de Honor" },
                    { "3", "Vicepresidencia de Honor" },
                    { "2", "Presidencia de Honor" },
                    { "9", "Coordinadora" },
                    { "1", "" }
                });

            migrationBuilder.InsertData(
                table: "TiposComite",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { "1", "" },
                    { "2", "Junta Directiva" },
                    { "3", "Comité de Honor" },
                    { "4", "Comité Organizador" },
                    { "5", "Comité Científico" },
                    { "6", "Comité Asesor" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "PuestosComite",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "TiposComite",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "TiposComite",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "TiposComite",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "TiposComite",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "TiposComite",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "TiposComite",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "TiposComite",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "PuestosComite",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9125), new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9139) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(8306), new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(8699) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9175), new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9180) });
        }
    }
}
