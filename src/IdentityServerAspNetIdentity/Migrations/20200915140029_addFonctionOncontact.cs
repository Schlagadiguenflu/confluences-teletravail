using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class addFonctionOncontact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "Fonction",
                table: "Contacts",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fonction",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Contacts",
                type: "character varying(13)",
                maxLength: 13,
                nullable: true);
        }
    }
}
