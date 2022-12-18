using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class TablaProveedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    CUENTACONTABLE = table.Column<string>(maxLength: 8, nullable: false),
                    NIF = table.Column<string>(maxLength: 17, nullable: false),
                    RAZONSOCIAL = table.Column<string>(maxLength: 120, nullable: false),
                    NOMBRECOMERCIAL = table.Column<string>(maxLength: 120, nullable: true),
                    DIRECCION = table.Column<string>(maxLength: 80, nullable: true),
                    CODPOST = table.Column<string>(maxLength: 10, nullable: false),
                    LOCALIDAD = table.Column<string>(maxLength: 30, nullable: true),
                    PROVINCIA = table.Column<string>(maxLength: 30, nullable: true),
                    PAIS = table.Column<string>(maxLength: 3, nullable: false),
                    PERSONACONTACTO = table.Column<string>(maxLength: 40, nullable: true),
                    CARGO = table.Column<string>(maxLength: 50, nullable: true),
                    EMAIL = table.Column<string>(maxLength: 150, nullable: true),
                    TELEFONO = table.Column<string>(maxLength: 15, nullable: true),
                    TELEFONO2 = table.Column<string>(maxLength: 15, nullable: true),
                    PAGINAWEB = table.Column<string>(maxLength: 60, nullable: true),
                    FORMAPAGO = table.Column<string>(maxLength: 2, nullable: true),
                    RETENCION = table.Column<string>(maxLength: 2, nullable: true),
                    COMISIONES = table.Column<string>(maxLength: 100, nullable: true),
                    OBSERVACIONES = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5262), new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5273) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(4662), new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(4948) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5289), new DateTime(2022, 7, 25, 19, 39, 27, 432, DateTimeKind.Local).AddTicks(5291) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4037), new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4045) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(3356), new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(3696) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4063), new DateTime(2022, 7, 20, 15, 15, 31, 423, DateTimeKind.Local).AddTicks(4065) });
        }
    }
}
