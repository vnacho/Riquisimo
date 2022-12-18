using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class encuentro_congressId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoEvento",
                table: "Encuentros");

            migrationBuilder.DropColumn(
                name: "NombreEvento",
                table: "Encuentros");

            migrationBuilder.AddColumn<Guid>(
                name: "CongressId",
                table: "Encuentros",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9565), new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9574) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(8924), new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9230) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9592), new DateTime(2022, 9, 6, 11, 44, 32, 713, DateTimeKind.Local).AddTicks(9595) });

            migrationBuilder.CreateIndex(
                name: "IX_Encuentros_CongressId",
                table: "Encuentros",
                column: "CongressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Encuentros_Congresses_CongressId",
                table: "Encuentros",
                column: "CongressId",
                principalTable: "Congresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encuentros_Congresses_CongressId",
                table: "Encuentros");

            migrationBuilder.DropIndex(
                name: "IX_Encuentros_CongressId",
                table: "Encuentros");

            migrationBuilder.DropColumn(
                name: "CongressId",
                table: "Encuentros");

            migrationBuilder.AddColumn<string>(
                name: "CodigoEvento",
                table: "Encuentros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreEvento",
                table: "Encuentros",
                type: "nvarchar(max)",
                nullable: true);

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
        }
    }
}
