using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class encuentros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "TIPO_RET",
                table: "Proveedores",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RAZONSOCIAL",
                table: "Proveedores",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "NOMBRECOMERCIAL",
                table: "Proveedores",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "MODO_RET",
                table: "Proveedores",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Encuentros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    CodigoEvento = table.Column<string>(nullable: false),
                    NombreEvento = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Lugar = table.Column<string>(nullable: false),
                    NumeroMesas = table.Column<int>(nullable: false),
                    ComensalesMesa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encuentros", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3500), new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3509) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(2863), new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3166) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3529), new DateTime(2022, 8, 31, 17, 46, 34, 0, DateTimeKind.Local).AddTicks(3532) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Encuentros");

            migrationBuilder.AlterColumn<bool>(
                name: "TIPO_RET",
                table: "Proveedores",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "RAZONSOCIAL",
                table: "Proveedores",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NOMBRECOMERCIAL",
                table: "Proveedores",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<bool>(
                name: "MODO_RET",
                table: "Proveedores",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(622), new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(632) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(1), new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(302) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(649), new DateTime(2022, 7, 29, 17, 23, 28, 608, DateTimeKind.Local).AddTicks(652) });
        }
    }
}
