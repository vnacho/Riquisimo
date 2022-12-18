using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class AddCommonData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "Registrations",
                defaultValue: "70100",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "Registrations",
                defaultValue: "I ",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Units",
                table: "Registrations",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceDataId",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Warehouse",
                table: "Congresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Vendedor",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "Accommodations",
                defaultValue: "70800",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "Accommodations",
                defaultValue: "A ",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Units",
                table: "Accommodations",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    CongressId = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: true),
                    BillingLocationId = table.Column<Guid>(nullable: true),
                    AccountId = table.Column<Guid>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    Serie = table.Column<string>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    Units = table.Column<double>(nullable: false),
                    VATId = table.Column<string>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    PaidDate = table.Column<DateTime>(nullable: true),
                    Imported = table.Column<bool>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    Notified = table.Column<bool>(nullable: false),
                    Authorization = table.Column<bool>(nullable: false),
                    OnlyBilling = table.Column<bool>(nullable: false),
                    Exported = table.Column<bool>(nullable: false),
                    Reviewed = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_ClientLocations_BillingLocationId",
                        column: x => x.BillingLocationId,
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_Congresses_CongressId",
                        column: x => x.CongressId,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    LogoBase64 = table.Column<string>(nullable: true),
                    TailBase64 = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    SageAccount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_AccountId",
                table: "Registrations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Congresses_InvoiceDataId",
                table: "Congresses",
                column: "InvoiceDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_AccountId",
                table: "Accommodations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BillingLocationId",
                table: "Expenses",
                column: "BillingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ClientId",
                table: "Expenses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CongressId",
                table: "Expenses",
                column: "CongressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Accounts_AccountId",
                table: "Accommodations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Congresses_InvoiceData_InvoiceDataId",
                table: "Congresses",
                column: "InvoiceDataId",
                principalTable: "InvoiceData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Accounts_AccountId",
                table: "Registrations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Accounts_AccountId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Congresses_InvoiceData_InvoiceDataId",
                table: "Congresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Accounts_AccountId",
                table: "Registrations");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "InvoiceData");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_AccountId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Congresses_InvoiceDataId",
                table: "Congresses");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_AccountId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "InvoiceDataId",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Warehouse",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Vendedor",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "Accommodations");
        }
    }
}
