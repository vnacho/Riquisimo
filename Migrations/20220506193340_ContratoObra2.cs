using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ContratoObra2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoCongress",
                table: "Congresses",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3801), new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3809) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(2554), new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3233) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3825), new DateTime(2022, 5, 6, 21, 33, 40, 456, DateTimeKind.Local).AddTicks(3828) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoCongress",
                table: "Congresses",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(199), new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(209) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 29, 12, 878, DateTimeKind.Local).AddTicks(9573), new DateTime(2022, 5, 6, 21, 29, 12, 878, DateTimeKind.Local).AddTicks(9875) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(226), new DateTime(2022, 5, 6, 21, 29, 12, 879, DateTimeKind.Local).AddTicks(228) });
        }
    }
}
