using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class obra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "Personal",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObraId",
                table: "Personal",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(461), new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(474) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 30, 12, 26, 3, 951, DateTimeKind.Local).AddTicks(9143), new DateTime(2021, 6, 30, 12, 26, 3, 951, DateTimeKind.Local).AddTicks(9829) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(496), new DateTime(2021, 6, 30, 12, 26, 3, 952, DateTimeKind.Local).AddTicks(499) });

            migrationBuilder.CreateIndex(
                name: "IX_Personal_ObraId",
                table: "Personal",
                column: "ObraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_CentrosCoste_ObraId",
                table: "Personal",
                column: "ObraId",
                principalTable: "CentrosCoste",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personal_CentrosCoste_ObraId",
                table: "Personal");

            migrationBuilder.DropIndex(
                name: "IX_Personal_ObraId",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "ObraId",
                table: "Personal");

            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "Personal",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9810), new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9821) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(8632), new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9198) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9844), new DateTime(2021, 6, 28, 9, 17, 20, 700, DateTimeKind.Local).AddTicks(9847) });
        }
    }
}
