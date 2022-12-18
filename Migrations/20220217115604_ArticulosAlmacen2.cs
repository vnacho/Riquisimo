using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ArticulosAlmacen2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticulosAlmacen",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosAlmacen", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1114), new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1123) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(516), new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(808) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1137), new DateTime(2022, 2, 17, 12, 56, 4, 122, DateTimeKind.Local).AddTicks(1139) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticulosAlmacen");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2097), new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2105) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(1459), new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(1761) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2121), new DateTime(2022, 2, 17, 12, 39, 52, 78, DateTimeKind.Local).AddTicks(2123) });
        }
    }
}
