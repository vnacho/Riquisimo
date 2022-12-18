using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class DocumentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "Registrations",
                nullable: false,
                defaultValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"));

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "Expenses",
                nullable: false,
                defaultValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"));

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "Accommodations",
                nullable: false,
                defaultValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"));

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "Created", "Deleted", "Modified", "Name" },
                values: new object[] { new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"), new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(190), null, new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(762), "Factura" });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "Created", "Deleted", "Modified", "Name" },
                values: new object[] { new Guid("043d73bf-1516-4672-affe-4f0836048f40"), new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1356), null, new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1367), "Factura proforma" });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "Created", "Deleted", "Modified", "Name" },
                values: new object[] { new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"), new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1387), null, new DateTime(2020, 4, 28, 16, 6, 33, 33, DateTimeKind.Local).AddTicks(1390), "Presupuesto" });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_DocumentTypeId",
                table: "Registrations",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_DocumentTypeId",
                table: "Expenses",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_DocumentTypeId",
                table: "Accommodations",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_DocumentTypes_DocumentTypeId",
                table: "Accommodations",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_DocumentTypes_DocumentTypeId",
                table: "Expenses",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_DocumentTypes_DocumentTypeId",
                table: "Registrations",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_DocumentTypes_DocumentTypeId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_DocumentTypes_DocumentTypeId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_DocumentTypes_DocumentTypeId",
                table: "Registrations");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_DocumentTypeId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_DocumentTypeId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_DocumentTypeId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "Accommodations");
        }
    }
}
