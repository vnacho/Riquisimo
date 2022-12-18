using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class categoriasinscritos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Creditos",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HaAsistido",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoriaInscritoId",
                table: "Registrant",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoriasInscritos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasInscritos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CategoriasInscritos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { "1", "Asistente" },
                    { "2", "Ponente" },
                    { "3", "Moderador" },
                    { "4", "Comité Científico" },
                    { "5", "Comité Organizador" },
                    { "6", "Junta Directiva" }
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 15, 12, 14, 31, 332, DateTimeKind.Local).AddTicks(2115), new DateTime(2022, 6, 15, 12, 14, 31, 332, DateTimeKind.Local).AddTicks(2130) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 15, 12, 14, 31, 332, DateTimeKind.Local).AddTicks(1125), new DateTime(2022, 6, 15, 12, 14, 31, 332, DateTimeKind.Local).AddTicks(1631) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 15, 12, 14, 31, 332, DateTimeKind.Local).AddTicks(2169), new DateTime(2022, 6, 15, 12, 14, 31, 332, DateTimeKind.Local).AddTicks(2174) });

            migrationBuilder.CreateIndex(
                name: "IX_Registrant_CategoriaInscritoId",
                table: "Registrant",
                column: "CategoriaInscritoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrant_CategoriasInscritos_CategoriaInscritoId",
                table: "Registrant",
                column: "CategoriaInscritoId",
                principalTable: "CategoriasInscritos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrant_CategoriasInscritos_CategoriaInscritoId",
                table: "Registrant");

            migrationBuilder.DropTable(
                name: "CategoriasInscritos");

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
                values: new object[] { new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8497), new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8506) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(7871), new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8170) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8524), new DateTime(2022, 6, 7, 17, 31, 19, 162, DateTimeKind.Local).AddTicks(8527) });
        }
    }
}