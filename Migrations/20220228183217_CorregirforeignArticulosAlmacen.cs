using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CorregirforeignArticulosAlmacen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 19, 32, 16, 820, DateTimeKind.Local).AddTicks(4308), new DateTime(2022, 2, 28, 19, 32, 16, 820, DateTimeKind.Local).AddTicks(4317) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 19, 32, 16, 820, DateTimeKind.Local).AddTicks(3703), new DateTime(2022, 2, 28, 19, 32, 16, 820, DateTimeKind.Local).AddTicks(3996) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 19, 32, 16, 820, DateTimeKind.Local).AddTicks(4335), new DateTime(2022, 2, 28, 19, 32, 16, 820, DateTimeKind.Local).AddTicks(4337) });

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosAlmacen_CentroCosteId",
                table: "ArticulosAlmacen",
                column: "CentroCosteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticulosAlmacen_CentrosCoste_CentroCosteId",
                table: "ArticulosAlmacen",
                column: "CentroCosteId",
                principalTable: "CentrosCoste",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticulosAlmacen_CentrosCoste_CentroCosteId",
                table: "ArticulosAlmacen");

            migrationBuilder.DropIndex(
                name: "IX_ArticulosAlmacen_CentroCosteId",
                table: "ArticulosAlmacen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3227), new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3237) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(2620), new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(2917) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3255), new DateTime(2022, 2, 28, 18, 57, 46, 147, DateTimeKind.Local).AddTicks(3258) });
        }
    }
}
