using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class inscripciones_campos_nuevos_especialidad_laboratorio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "Registrant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Laboratorio",
                table: "Registrant",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 20, 17, 32, 18, 292, DateTimeKind.Local).AddTicks(8013), new DateTime(2022, 6, 20, 17, 32, 18, 292, DateTimeKind.Local).AddTicks(8027) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 20, 17, 32, 18, 292, DateTimeKind.Local).AddTicks(7262), new DateTime(2022, 6, 20, 17, 32, 18, 292, DateTimeKind.Local).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 20, 17, 32, 18, 292, DateTimeKind.Local).AddTicks(8052), new DateTime(2022, 6, 20, 17, 32, 18, 292, DateTimeKind.Local).AddTicks(8057) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "Registrant");

            migrationBuilder.DropColumn(
                name: "Laboratorio",
                table: "Registrant");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4313), new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4321) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(3618), new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4340), new DateTime(2022, 6, 16, 12, 31, 1, 32, DateTimeKind.Local).AddTicks(4342) });
        }
    }
}
