using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class uniqueTypeMetier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TypeMetiers_Code",
                table: "TypeMetiers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeMetiers_Libelle",
                table: "TypeMetiers",
                column: "Libelle",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TypeMetiers_Code",
                table: "TypeMetiers");

            migrationBuilder.DropIndex(
                name: "IX_TypeMetiers_Libelle",
                table: "TypeMetiers");

        }
    }
}
