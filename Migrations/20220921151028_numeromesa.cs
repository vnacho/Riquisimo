using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class numeromesa : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "NumeroMesa",
                table: "Restauraciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 21, 17, 10, 27, 548, DateTimeKind.Local).AddTicks(3137), new DateTime(2022, 9, 21, 17, 10, 27, 548, DateTimeKind.Local).AddTicks(3145) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 21, 17, 10, 27, 548, DateTimeKind.Local).AddTicks(2469), new DateTime(2022, 9, 21, 17, 10, 27, 548, DateTimeKind.Local).AddTicks(2774) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 21, 17, 10, 27, 548, DateTimeKind.Local).AddTicks(3163), new DateTime(2022, 9, 21, 17, 10, 27, 548, DateTimeKind.Local).AddTicks(3166) });

            migrationBuilder.CreateIndex(
                name: "IX_Ponentes_CongressId",
                table: "Ponentes",
                column: "CongressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ponentes_Congresses_CongressId",
                table: "Ponentes",
                column: "CongressId",
                principalTable: "Congresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ponentes_Congresses_CongressId",
                table: "Ponentes");

            migrationBuilder.DropIndex(
                name: "IX_Ponentes_CongressId",
                table: "Ponentes");

            migrationBuilder.DropColumn(
                name: "NumeroMesa",
                table: "Restauraciones");

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
