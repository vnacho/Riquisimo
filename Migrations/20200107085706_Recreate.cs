using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class Recreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    SageCode = table.Column<string>(nullable: true),
                    BusinessName = table.Column<string>(nullable: false),
                    NIF = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Email2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Congresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LogoBase64 = table.Column<string>(nullable: true),
                    TailBase64 = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 5, nullable: false),
                    ConnectionString = table.Column<string>(nullable: true),
                    HideRegistrations = table.Column<bool>(nullable: false),
                    CertificateNumber = table.Column<string>(nullable: true),
                    CertificateCreditor = table.Column<string>(nullable: true),
                    CertificateCredits = table.Column<double>(nullable: false),
                    CertificateCity = table.Column<string>(nullable: true),
                    CertificateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Congresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrantLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Phone2 = table.Column<string>(nullable: true),
                    RegistrantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrantLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Phone2 = table.Column<string>(nullable: true),
                    SageLine = table.Column<int>(nullable: true),
                    SageClient = table.Column<string>(nullable: true),
                    ClientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    CongressId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Congresses_CongressId",
                        column: x => x.CongressId,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registrant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Surnames = table.Column<string>(nullable: false),
                    TreatmentId = table.Column<Guid>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    ProfessionalCategory = table.Column<string>(nullable: true),
                    Workplace = table.Column<string>(nullable: true),
                    LocationId = table.Column<Guid>(nullable: false),
                    NIF = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Email2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrant_RegistrantLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "RegistrantLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrant_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    CongressId = table.Column<Guid>(nullable: false),
                    RegistrantId = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    VATId = table.Column<string>(nullable: false),
                    BillingLocationId = table.Column<Guid>(nullable: true),
                    Notified = table.Column<bool>(nullable: false),
                    Authorization = table.Column<bool>(nullable: false),
                    OnlyBilling = table.Column<bool>(nullable: false),
                    Exported = table.Column<bool>(nullable: false),
                    Imported = table.Column<bool>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    Reviewed = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    CompanionId = table.Column<Guid>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Hotel = table.Column<string>(nullable: false),
                    RoomTypeId = table.Column<Guid>(nullable: false),
                    Occupants = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodations_ClientLocations_BillingLocationId",
                        column: x => x.BillingLocationId,
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accommodations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accommodations_Registrant_CompanionId",
                        column: x => x.CompanionId,
                        principalTable: "Registrant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accommodations_Congresses_CongressId",
                        column: x => x.CongressId,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accommodations_Registrant_RegistrantId",
                        column: x => x.RegistrantId,
                        principalTable: "Registrant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accommodations_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    CongressId = table.Column<Guid>(nullable: false),
                    RegistrantId = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    VATId = table.Column<string>(nullable: false),
                    BillingLocationId = table.Column<Guid>(nullable: true),
                    Notified = table.Column<bool>(nullable: false),
                    Authorization = table.Column<bool>(nullable: false),
                    OnlyBilling = table.Column<bool>(nullable: false),
                    Exported = table.Column<bool>(nullable: false),
                    Imported = table.Column<bool>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    Reviewed = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_ClientLocations_BillingLocationId",
                        column: x => x.BillingLocationId,
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Congresses_CongressId",
                        column: x => x.CongressId,
                        principalTable: "Congresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_Registrant_RegistrantId",
                        column: x => x.RegistrantId,
                        principalTable: "Registrant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_RegistrationTypes_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_BillingLocationId",
                table: "Accommodations",
                column: "BillingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_ClientId",
                table: "Accommodations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_CompanionId",
                table: "Accommodations",
                column: "CompanionId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_CongressId",
                table: "Accommodations",
                column: "CongressId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_RegistrantId",
                table: "Accommodations",
                column: "RegistrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_RoomTypeId",
                table: "Accommodations",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CongressId",
                table: "Accounts",
                column: "CongressId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_ClientId",
                table: "ClientLocations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrant_LocationId",
                table: "Registrant",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrant_TreatmentId",
                table: "Registrant",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_BillingLocationId",
                table: "Registrations",
                column: "BillingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ClientId",
                table: "Registrations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_CongressId",
                table: "Registrations",
                column: "CongressId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_RegistrantId",
                table: "Registrations",
                column: "RegistrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_RegistrationTypeId",
                table: "Registrations",
                column: "RegistrationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropTable(
                name: "ClientLocations");

            migrationBuilder.DropTable(
                name: "Congresses");

            migrationBuilder.DropTable(
                name: "Registrant");

            migrationBuilder.DropTable(
                name: "RegistrationTypes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "RegistrantLocations");

            migrationBuilder.DropTable(
                name: "Treatments");
        }
    }
}
