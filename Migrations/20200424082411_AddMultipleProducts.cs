using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AddMultipleProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Warehouse",
                table: "Congresses");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    ExpenseId = table.Column<Guid>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Serie = table.Column<string>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    Units = table.Column<double>(nullable: false),
                    VATId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ExpenseId",
                table: "Product",
                column: "ExpenseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.AddColumn<int>(
                name: "Warehouse",
                table: "Congresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
