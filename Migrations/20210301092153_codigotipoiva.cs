using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class codigotipoiva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoIVA",
                table: "CompraFacturaLineas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3706), new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3715) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(2470), new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3030) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3735), new DateTime(2021, 3, 1, 10, 21, 52, 843, DateTimeKind.Local).AddTicks(3738) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoTipoIVA",
                table: "CompraFacturaLineas");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8861), new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8872) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(7500), new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8096) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8915), new DateTime(2021, 2, 23, 19, 18, 33, 148, DateTimeKind.Local).AddTicks(8918) });
        }
    }
}
