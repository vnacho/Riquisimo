using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class documentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CT",
                table: "Personal",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaApto",
                table: "Personal",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaValidezNIF",
                table: "Personal",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RevisionMedica",
                table: "Personal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SAP",
                table: "Personal",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Venta",
                table: "Personal",
                type: "decimal(13,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "GrabacionE9",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FicheroUrl = table.Column<string>(nullable: true),
                    FicheroNombre = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    PersonalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documento_Personal_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9810), new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9821) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(8632), new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9198) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9844), new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9847) });

            migrationBuilder.CreateIndex(
                name: "IX_Documento_PersonalId",
                table: "Documento",
                column: "PersonalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropColumn(
                name: "CT",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "FechaApto",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "FechaValidezNIF",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "RevisionMedica",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "SAP",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "Venta",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "GrabacionE9");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4751), new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4773) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(2863), new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(3711) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4920), new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4924) });
        }
    }
}
