using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class restauracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restauraciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EncuentroId = table.Column<int>(nullable: false),
                    RegistrationId = table.Column<Guid>(nullable: false),
                    FechaHoraReserva = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restauraciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restauraciones_Encuentros_EncuentroId",
                        column: x => x.EncuentroId,
                        principalTable: "Encuentros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restauraciones_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 55, 40, 616, DateTimeKind.Local).AddTicks(7745), new DateTime(2022, 8, 31, 18, 55, 40, 616, DateTimeKind.Local).AddTicks(7754) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 55, 40, 616, DateTimeKind.Local).AddTicks(7126), new DateTime(2022, 8, 31, 18, 55, 40, 616, DateTimeKind.Local).AddTicks(7423) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 55, 40, 616, DateTimeKind.Local).AddTicks(7770), new DateTime(2022, 8, 31, 18, 55, 40, 616, DateTimeKind.Local).AddTicks(7773) });

            migrationBuilder.CreateIndex(
                name: "IX_Restauraciones_EncuentroId",
                table: "Restauraciones",
                column: "EncuentroId");

            migrationBuilder.CreateIndex(
                name: "IX_Restauraciones_RegistrationId",
                table: "Restauraciones",
                column: "RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restauraciones");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3368), new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3376) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(2750), new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3048) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3392), new DateTime(2022, 8, 31, 18, 12, 29, 295, DateTimeKind.Local).AddTicks(3395) });
        }
    }
}
