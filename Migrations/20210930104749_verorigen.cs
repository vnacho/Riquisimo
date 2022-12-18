using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class verorigen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddOrigen",
                table: "TiposCentroCoste");

            migrationBuilder.CreateTable(
                name: "VerOrigen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivelAnalitico1 = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerOrigen", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3014), new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3023) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(2215), new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(2587) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3045), new DateTime(2021, 9, 30, 12, 47, 49, 228, DateTimeKind.Local).AddTicks(3048) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerOrigen");

            migrationBuilder.AddColumn<bool>(
                name: "AddOrigen",
                table: "TiposCentroCoste",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5806), new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5816) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5019), new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5394) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5838), new DateTime(2021, 9, 30, 12, 7, 42, 792, DateTimeKind.Local).AddTicks(5841) });
        }
    }
}
