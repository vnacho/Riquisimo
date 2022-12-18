using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class tratamiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tratamiento",
                table: "Asistente");

            migrationBuilder.AddColumn<Guid>(
                name: "TreatmentId",
                table: "Asistente",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8173), new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8186) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(7173), new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(7619) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8210), new DateTime(2021, 10, 4, 15, 16, 26, 384, DateTimeKind.Local).AddTicks(8213) });

            migrationBuilder.CreateIndex(
                name: "IX_Asistente_TreatmentId",
                table: "Asistente",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistente_Treatments_TreatmentId",
                table: "Asistente",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistente_Treatments_TreatmentId",
                table: "Asistente");

            migrationBuilder.DropIndex(
                name: "IX_Asistente_TreatmentId",
                table: "Asistente");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "Asistente");

            migrationBuilder.AddColumn<int>(
                name: "Tratamiento",
                table: "Asistente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5297), new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5306) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(4129), new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(4698) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5326), new DateTime(2021, 9, 30, 14, 16, 12, 258, DateTimeKind.Local).AddTicks(5329) });
        }
    }
}
