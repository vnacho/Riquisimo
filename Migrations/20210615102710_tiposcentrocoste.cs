using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tiposcentrocoste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposCentroCoste",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(maxLength: 1, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposCentroCoste", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 12, 27, 10, 365, DateTimeKind.Local).AddTicks(6493), new DateTime(2021, 6, 15, 12, 27, 10, 365, DateTimeKind.Local).AddTicks(6504) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 12, 27, 10, 365, DateTimeKind.Local).AddTicks(5308), new DateTime(2021, 6, 15, 12, 27, 10, 365, DateTimeKind.Local).AddTicks(5876) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 12, 27, 10, 365, DateTimeKind.Local).AddTicks(6525), new DateTime(2021, 6, 15, 12, 27, 10, 365, DateTimeKind.Local).AddTicks(6529) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiposCentroCoste");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5895), new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5909) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(4500), new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5065) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5932), new DateTime(2021, 5, 7, 12, 14, 26, 941, DateTimeKind.Local).AddTicks(5935) });
        }
    }
}
