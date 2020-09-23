using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class AddedUniqueOnAllTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TypeStages_Nom",
                table: "TypeStages",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeOffres_Libelle",
                table: "TypeOffres",
                column: "Libelle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeMoyens_Code",
                table: "TypeMoyens",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeMoyens_Libelle",
                table: "TypeMoyens",
                column: "Libelle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeEntreprises_Nom",
                table: "TypeEntreprises",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeDomaines_Code",
                table: "TypeDomaines",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeDomaines_Libelle",
                table: "TypeDomaines",
                column: "Libelle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeAnnonces_Libelle",
                table: "TypeAnnonces",
                column: "Libelle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeAffiliations_Code",
                table: "TypeAffiliations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeAffiliations_Libelle",
                table: "TypeAffiliations",
                column: "Libelle",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TypeStages_Nom",
                table: "TypeStages");

            migrationBuilder.DropIndex(
                name: "IX_TypeOffres_Libelle",
                table: "TypeOffres");

            migrationBuilder.DropIndex(
                name: "IX_TypeMoyens_Code",
                table: "TypeMoyens");

            migrationBuilder.DropIndex(
                name: "IX_TypeMoyens_Libelle",
                table: "TypeMoyens");

            migrationBuilder.DropIndex(
                name: "IX_TypeEntreprises_Nom",
                table: "TypeEntreprises");

            migrationBuilder.DropIndex(
                name: "IX_TypeDomaines_Code",
                table: "TypeDomaines");

            migrationBuilder.DropIndex(
                name: "IX_TypeDomaines_Libelle",
                table: "TypeDomaines");

            migrationBuilder.DropIndex(
                name: "IX_TypeAnnonces_Libelle",
                table: "TypeAnnonces");

            migrationBuilder.DropIndex(
                name: "IX_TypeAffiliations_Code",
                table: "TypeAffiliations");

            migrationBuilder.DropIndex(
                name: "IX_TypeAffiliations_Libelle",
                table: "TypeAffiliations");
        }
    }
}
