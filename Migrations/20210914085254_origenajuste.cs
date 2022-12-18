using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class origenajuste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Origen_TiposCentroCoste_TipoCentroCosteId",
                table: "Origen");

            migrationBuilder.DropIndex(
                name: "IX_Origen_TipoCentroCosteId",
                table: "Origen");

            migrationBuilder.DropColumn(
                name: "TipoCentroCosteId",
                table: "Origen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(412), new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(423) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 52, 53, 658, DateTimeKind.Local).AddTicks(9579), new DateTime(2021, 9, 14, 10, 52, 53, 658, DateTimeKind.Local).AddTicks(9950) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(445), new DateTime(2021, 9, 14, 10, 52, 53, 659, DateTimeKind.Local).AddTicks(448) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoCentroCosteId",
                table: "Origen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(390), new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(400) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 36, 36, 45, DateTimeKind.Local).AddTicks(9556), new DateTime(2021, 9, 14, 10, 36, 36, 45, DateTimeKind.Local).AddTicks(9958) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(423), new DateTime(2021, 9, 14, 10, 36, 36, 46, DateTimeKind.Local).AddTicks(427) });

            migrationBuilder.CreateIndex(
                name: "IX_Origen_TipoCentroCosteId",
                table: "Origen",
                column: "TipoCentroCosteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Origen_TiposCentroCoste_TipoCentroCosteId",
                table: "Origen",
                column: "TipoCentroCosteId",
                principalTable: "TiposCentroCoste",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
