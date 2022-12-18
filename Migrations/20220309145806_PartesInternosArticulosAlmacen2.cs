using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class PartesInternosArticulosAlmacen2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartesInternosAlmacen",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    fecha = table.Column<DateTime>(nullable: false),
                    ArticulosAlmacenId = table.Column<int>(nullable: false),
                    ArticulosAlmacenId1 = table.Column<Guid>(nullable: true),
                    TariffTypeUnits = table.Column<decimal>(nullable: false),
                    TariffTypeUnits2 = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    DestinoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartesInternosAlmacen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartesInternosAlmacen_ArticulosAlmacen_ArticulosAlmacenId1",
                        column: x => x.ArticulosAlmacenId1,
                        principalTable: "ArticulosAlmacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartesInternosAlmacen_CentrosCoste_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "CentrosCoste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6318), new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6327) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(5680), new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(5987) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6362), new DateTime(2022, 3, 9, 15, 58, 6, 511, DateTimeKind.Local).AddTicks(6364) });

            migrationBuilder.CreateIndex(
                name: "IX_PartesInternosAlmacen_ArticulosAlmacenId1",
                table: "PartesInternosAlmacen",
                column: "ArticulosAlmacenId1");

            migrationBuilder.CreateIndex(
                name: "IX_PartesInternosAlmacen_DestinoId",
                table: "PartesInternosAlmacen",
                column: "DestinoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartesInternosAlmacen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4918), new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4927) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4309), new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4604) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4943), new DateTime(2022, 3, 8, 14, 56, 14, 597, DateTimeKind.Local).AddTicks(4945) });
        }
    }
}
