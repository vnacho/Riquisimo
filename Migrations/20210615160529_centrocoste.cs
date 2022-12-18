using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class centrocoste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CentrosCoste",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivelAnalitico1 = table.Column<string>(maxLength: 3, nullable: false),
                    NivelAnalitico2 = table.Column<string>(maxLength: 5, nullable: false),
                    Nombre = table.Column<string>(maxLength: 80, nullable: false),
                    TipoCentroCosteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosCoste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentrosCoste_TiposCentroCoste_TipoCentroCosteId",
                        column: x => x.TipoCentroCosteId,
                        principalTable: "TiposCentroCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 18, 5, 28, 520, DateTimeKind.Local).AddTicks(6740), new DateTime(2021, 6, 15, 18, 5, 28, 520, DateTimeKind.Local).AddTicks(6749) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 18, 5, 28, 520, DateTimeKind.Local).AddTicks(5637), new DateTime(2021, 6, 15, 18, 5, 28, 520, DateTimeKind.Local).AddTicks(6176) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 15, 18, 5, 28, 520, DateTimeKind.Local).AddTicks(6770), new DateTime(2021, 6, 15, 18, 5, 28, 520, DateTimeKind.Local).AddTicks(6773) });

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCoste_TipoCentroCosteId",
                table: "CentrosCoste",
                column: "TipoCentroCosteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentrosCoste");

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
    }
}
