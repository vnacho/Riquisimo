using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ampliacionespersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUltimoContrato",
                table: "Personal",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoUltimoContratoId",
                table: "Personal",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TipoContrato",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoContrato", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7254), new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7267) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(6338), new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(6775) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7290), new DateTime(2021, 10, 14, 18, 25, 10, 226, DateTimeKind.Local).AddTicks(7294) });

            migrationBuilder.CreateIndex(
                name: "IX_Personal_TipoUltimoContratoId",
                table: "Personal",
                column: "TipoUltimoContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_TipoContrato_TipoUltimoContratoId",
                table: "Personal",
                column: "TipoUltimoContratoId",
                principalTable: "TipoContrato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personal_TipoContrato_TipoUltimoContratoId",
                table: "Personal");

            migrationBuilder.DropTable(
                name: "TipoContrato");

            migrationBuilder.DropIndex(
                name: "IX_Personal_TipoUltimoContratoId",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "FechaUltimoContrato",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "TipoUltimoContratoId",
                table: "Personal");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 17, 32, 10, 768, DateTimeKind.Local).AddTicks(1852), new DateTime(2021, 10, 4, 17, 32, 10, 768, DateTimeKind.Local).AddTicks(1862) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 17, 32, 10, 768, DateTimeKind.Local).AddTicks(1007), new DateTime(2021, 10, 4, 17, 32, 10, 768, DateTimeKind.Local).AddTicks(1404) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 17, 32, 10, 768, DateTimeKind.Local).AddTicks(1884), new DateTime(2021, 10, 4, 17, 32, 10, 768, DateTimeKind.Local).AddTicks(1887) });
        }
    }
}
