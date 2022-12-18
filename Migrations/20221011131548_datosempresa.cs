using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class datosempresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(52), new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(62) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 15, 48, 154, DateTimeKind.Local).AddTicks(9377), new DateTime(2022, 10, 11, 15, 15, 48, 154, DateTimeKind.Local).AddTicks(9701) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(82), new DateTime(2022, 10, 11, 15, 15, 48, 155, DateTimeKind.Local).AddTicks(84) });

            migrationBuilder.InsertData(
                table: "Parametros",
                columns: new[] { "Codigo", "Descripcion", "Formato", "Valor" },
                values: new object[,]
                {
                    { "EMPRESA_NOMBRE", "Indica el nombre de la empresa.", null, null },
                    { "EMPRESA_DIRECCION", "Indica la dirección de la empresa.", null, null },
                    { "EMPRESA_CP", "Indica el código postal de la empresa.", null, null },
                    { "EMPRESA_POBLACION", "Indica la población de la empresa.", null, null },
                    { "EMPRESA_PROVINCIA", "Indica la provincia de la empresa.", null, null },
                    { "EMPRESA_NIF_CIF", "Indica el NIF/CIF de la empresa.", null, null },
                    { "EMPRESA_LOGO", "Respresenta el logo de la empresa.", "Base64", null },
                    { "EMPRESA_FIRMA", "Respresenta la firma de la empresa.", "Base64", null },
                    { "EMPRESA_REPRESENTANTE", "Indica el nombre del representante de la empresa.", null, null },
                    { "EMPRESA_NIF_CIF_REPRESENTANTE", "Indica el NIF/CIF del representante de la empresa.", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_CP");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_DIRECCION");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_FIRMA");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_LOGO");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_NIF_CIF");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_NIF_CIF_REPRESENTANTE");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_NOMBRE");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_POBLACION");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_PROVINCIA");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Codigo",
                keyValue: "EMPRESA_REPRESENTANTE");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9239), new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(8614), new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(8915) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9270), new DateTime(2022, 10, 6, 10, 12, 12, 97, DateTimeKind.Local).AddTicks(9272) });
        }
    }
}
