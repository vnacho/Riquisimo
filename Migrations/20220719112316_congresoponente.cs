using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class congresoponente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CongressId",
                table: "Ponentes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9125), new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9139) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(8306), new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(8699) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9175), new DateTime(2022, 7, 19, 13, 23, 16, 154, DateTimeKind.Local).AddTicks(9180) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CongressId",
                table: "Ponentes");

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
    }
}
