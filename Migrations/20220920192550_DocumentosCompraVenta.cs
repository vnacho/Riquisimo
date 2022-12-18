using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class DocumentosCompraVenta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "Congresses",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Congresses",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "contraseña",
                table: "Congresses",
                newName: "Contraseña");

            migrationBuilder.CreateTable(
                name: "DocumentoCompraVenta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FicheroUrl = table.Column<string>(nullable: true),
                    FicheroNombre = table.Column<string>(nullable: true),
                    ClaveDoc = table.Column<string>(maxLength: 2, nullable: true),
                    IdTabla = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoCompraVenta", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 20, 21, 25, 49, 288, DateTimeKind.Local).AddTicks(1396), new DateTime(2022, 9, 20, 21, 25, 49, 288, DateTimeKind.Local).AddTicks(1405) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 20, 21, 25, 49, 288, DateTimeKind.Local).AddTicks(739), new DateTime(2022, 9, 20, 21, 25, 49, 288, DateTimeKind.Local).AddTicks(1069) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 20, 21, 25, 49, 288, DateTimeKind.Local).AddTicks(1424), new DateTime(2022, 9, 20, 21, 25, 49, 288, DateTimeKind.Local).AddTicks(1427) });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "DocumentoCompraVenta");

            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "Congresses",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Congresses",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Contraseña",
                table: "Congresses",
                newName: "contraseña");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9272), new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9285) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(8366), new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(8741) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9321), new DateTime(2022, 9, 17, 7, 6, 7, 830, DateTimeKind.Local).AddTicks(9325) });
        }
    }
}
