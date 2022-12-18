using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class codigospedidoalbarannumerico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CodigoPedido",
                table: "VentaPedidos",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CodigoAlbaran",
                table: "VentaAlbaranes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8881), new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8890) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8237), new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8546) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8908), new DateTime(2022, 2, 2, 14, 44, 19, 183, DateTimeKind.Local).AddTicks(8910) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CodigoPedido",
                table: "VentaPedidos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "CodigoAlbaran",
                table: "VentaAlbaranes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

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
    }
}
