using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class grabaciondestino : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinoId",
                table: "GrabacionE9",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4751), new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4773) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(2863), new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(3711) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4920), new DateTime(2021, 6, 18, 17, 5, 27, 505, DateTimeKind.Local).AddTicks(4924) });

            migrationBuilder.CreateIndex(
                name: "IX_GrabacionE9_DestinoId",
                table: "GrabacionE9",
                column: "DestinoId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrabacionE9_CentrosCoste_DestinoId",
                table: "GrabacionE9",
                column: "DestinoId",
                principalTable: "CentrosCoste",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrabacionE9_CentrosCoste_DestinoId",
                table: "GrabacionE9");

            migrationBuilder.DropIndex(
                name: "IX_GrabacionE9_DestinoId",
                table: "GrabacionE9");

            migrationBuilder.DropColumn(
                name: "DestinoId",
                table: "GrabacionE9");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6711), new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6725) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(5505), new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6091) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6748), new DateTime(2021, 6, 18, 15, 9, 42, 19, DateTimeKind.Local).AddTicks(6751) });
        }
    }
}
