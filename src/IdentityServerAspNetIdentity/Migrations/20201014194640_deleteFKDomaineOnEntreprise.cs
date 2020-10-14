using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class deleteFKDomaineOnEntreprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entreprises_TypeDomaines_TypeDomaineId",
                table: "Entreprises");

            migrationBuilder.DropIndex(
                name: "IX_Entreprises_TypeDomaineId",
                table: "Entreprises");

            migrationBuilder.DropColumn(
                name: "TypeDomaineId",
                table: "Entreprises");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeDomaineId",
                table: "Entreprises",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_TypeDomaineId",
                table: "Entreprises",
                column: "TypeDomaineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entreprises_TypeDomaines_TypeDomaineId",
                table: "Entreprises",
                column: "TypeDomaineId",
                principalTable: "TypeDomaines",
                principalColumn: "TypeDomaineId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
