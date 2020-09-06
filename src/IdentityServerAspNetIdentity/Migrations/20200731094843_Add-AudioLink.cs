using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class AddAudioLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioLink",
                table: "Theories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioLink",
                table: "Exercices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioLink",
                table: "Theories");

            migrationBuilder.DropColumn(
                name: "AudioLink",
                table: "Exercices");
        }
    }
}
