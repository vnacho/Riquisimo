using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class CongressEmails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CongressEmailAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    CongressId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    IncomingMailServer = table.Column<string>(nullable: true),
                    OutgoingMailServer = table.Column<string>(nullable: true),
                    IncomingMailPort = table.Column<int>(nullable: false),
                    OutgoingMailPort = table.Column<int>(nullable: false),
                    MailUser = table.Column<string>(nullable: true),
                    MailPassword = table.Column<string>(nullable: true),
                    SignatureBefore = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(nullable: true),
                    SignatureAfter = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongressEmailAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CongressEmailAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CongressEmailAccounts_Congresses_CongressId",
                        column: x => x.CongressId,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8392), new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8403) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(7264), new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(7818) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8424), new DateTime(2020, 4, 30, 17, 29, 18, 988, DateTimeKind.Local).AddTicks(8427) });

            migrationBuilder.CreateIndex(
                name: "IX_CongressEmailAccounts_AccountId",
                table: "CongressEmailAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CongressEmailAccounts_CongressId_AccountId",
                table: "CongressEmailAccounts",
                columns: new[] { "CongressId", "AccountId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CongressEmailAccounts");

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1654), new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1668) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(551), new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1094) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1688), new DateTime(2020, 4, 29, 16, 2, 9, 870, DateTimeKind.Local).AddTicks(1691) });
        }
    }
}
