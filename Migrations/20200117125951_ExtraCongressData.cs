using Microsoft.EntityFrameworkCore.Migrations;

namespace Ferpuser.Migrations
{
    public partial class ExtraCongressData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attendants",
                table: "Congresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "Congresses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Catering",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationsAndSocialMedia",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationsAndType",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporatePhone",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email2",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Financing",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headquarters",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Infrastructures",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InscriptionFee",
                table: "Congresses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "InternalCode",
                table: "Congresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MailPassword",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MailPort",
                table: "Congresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MailServer",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailUser",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhone",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "President",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScientificSecretariat",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpeakersHotel",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalSecretariat",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Travels",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebDomain",
                table: "Congresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Workplace",
                table: "Congresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendants",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Catering",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "CommunicationsAndSocialMedia",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "CommunicationsAndType",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "CorporatePhone",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Email2",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Financing",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Headquarters",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Infrastructures",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "InscriptionFee",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "MailPassword",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "MailPort",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "MailServer",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "MailUser",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "PersonalPhone",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "President",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "ScientificSecretariat",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "SpeakersHotel",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "TechnicalSecretariat",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Travels",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "WebDomain",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "Workplace",
                table: "Congresses");
        }
    }
}
