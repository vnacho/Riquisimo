using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class codigospedidoalbaran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoPedido",
                table: "VentaPedidos",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoAlbaran",
                table: "VentaAlbaranes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 27, 24, 991, DateTimeKind.Local).AddTicks(7294), new DateTime(2022, 2, 2, 14, 27, 24, 991, DateTimeKind.Local).AddTicks(7302) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 27, 24, 991, DateTimeKind.Local).AddTicks(6657), new DateTime(2022, 2, 2, 14, 27, 24, 991, DateTimeKind.Local).AddTicks(6965) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 27, 24, 991, DateTimeKind.Local).AddTicks(7319), new DateTime(2022, 2, 2, 14, 27, 24, 991, DateTimeKind.Local).AddTicks(7322) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoPedido",
                table: "VentaPedidos");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoAlbaran",
                table: "VentaAlbaranes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3552), new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3562) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(2916), new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3224) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3581), new DateTime(2022, 1, 26, 18, 35, 10, 465, DateTimeKind.Local).AddTicks(3583) });
        }
    }
}
