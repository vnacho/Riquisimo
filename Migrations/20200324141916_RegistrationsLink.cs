using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class RegistrationsLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccommodationId",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationId",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_RegistrationId",
                table: "Accommodations",
                column: "RegistrationId",
                unique: true,
                filter: "[RegistrationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Registrations_RegistrationId",
                table: "Accommodations",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Registrations_RegistrationId",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_RegistrationId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "AccommodationId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "Accommodations");
        }
    }
}
