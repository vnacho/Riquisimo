using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tiendaTikect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tiendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    nombre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tikects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    tiendaID = table.Column<Guid>(nullable: false),
                    FechaTikect = table.Column<DateTime>(nullable: true),
                    importe = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tikects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tikects_Tiendas_tiendaID",
                        column: x => x.tiendaID,
                        principalTable: "Tiendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7489), new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7500) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(6827), new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7147) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7517), new DateTime(2022, 12, 4, 20, 13, 55, 280, DateTimeKind.Local).AddTicks(7519) });

            migrationBuilder.CreateIndex(
                name: "IX_Tikects_tiendaID",
                table: "Tikects",
                column: "tiendaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tikects");

            migrationBuilder.DropTable(
                name: "Tiendas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5703), new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5714) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(4943), new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5268) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5733), new DateTime(2022, 11, 10, 14, 30, 31, 761, DateTimeKind.Local).AddTicks(5735) });
        }
    }
}
