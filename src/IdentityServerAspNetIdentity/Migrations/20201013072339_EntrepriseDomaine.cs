using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class EntrepriseDomaine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntrepriseDomaines",
                columns: table => new
                {
                    EntrepriseId = table.Column<int>(nullable: false),
                    TypeDomaineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrepriseDomaines", x => new { x.EntrepriseId, x.TypeDomaineId });
                    table.ForeignKey(
                        name: "FK_EntrepriseDomaines_Entreprises_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprises",
                        principalColumn: "EntrepriseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrepriseDomaines_TypeDomaines_TypeDomaineId",
                        column: x => x.TypeDomaineId,
                        principalTable: "TypeDomaines",
                        principalColumn: "TypeDomaineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntrepriseDomaines_TypeDomaineId",
                table: "EntrepriseDomaines",
                column: "TypeDomaineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntrepriseDomaines");
        }
    }
}
