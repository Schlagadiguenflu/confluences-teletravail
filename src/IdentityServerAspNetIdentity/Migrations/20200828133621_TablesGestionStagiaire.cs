using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class TablesGestionStagiaire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeAffiliationId",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeAffiliations",
                columns: table => new
                {
                    TypeAffiliationId = table.Column<string>(maxLength: 10, nullable: false),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Libelle = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAffiliations", x => x.TypeAffiliationId);
                });

            migrationBuilder.CreateTable(
                name: "TypeAnnonces",
                columns: table => new
                {
                    TypeAnnonceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Libelle = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAnnonces", x => x.TypeAnnonceId);
                });

            migrationBuilder.CreateTable(
                name: "TypeDomaines",
                columns: table => new
                {
                    TypeDomaineId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(maxLength: 3, nullable: true),
                    Libelle = table.Column<string>(maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDomaines", x => x.TypeDomaineId);
                });

            migrationBuilder.CreateTable(
                name: "TypeEntreprises",
                columns: table => new
                {
                    TypeEntrepriseId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEntreprises", x => x.TypeEntrepriseId);
                });

            migrationBuilder.CreateTable(
                name: "TypeMetiers",
                columns: table => new
                {
                    TypeMetierId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Libelle = table.Column<string>(maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeMetiers", x => x.TypeMetierId);
                });

            migrationBuilder.CreateTable(
                name: "TypeMoyens",
                columns: table => new
                {
                    TypeMoyenId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(maxLength: 3, nullable: true),
                    Libelle = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeMoyens", x => x.TypeMoyenId);
                });

            migrationBuilder.CreateTable(
                name: "TypeOffres",
                columns: table => new
                {
                    TypeOffreId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Libelle = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOffres", x => x.TypeOffreId);
                });

            migrationBuilder.CreateTable(
                name: "TypeStages",
                columns: table => new
                {
                    TypeStageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeStages", x => x.TypeStageId);
                });

            migrationBuilder.CreateTable(
                name: "Entreprises",
                columns: table => new
                {
                    EntrepriseId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(maxLength: 50, nullable: false),
                    Ville = table.Column<string>(maxLength: 50, nullable: false),
                    TelFix = table.Column<string>(maxLength: 13, nullable: true),
                    TelFax = table.Column<string>(maxLength: 13, nullable: true),
                    Adr1 = table.Column<string>(maxLength: 50, nullable: true),
                    Adr2 = table.Column<string>(maxLength: 50, nullable: true),
                    CodePostal = table.Column<string>(maxLength: 4, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Remarque = table.Column<string>(maxLength: 10000, nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: true),
                    TypeEntrepriseId = table.Column<int>(nullable: true),
                    TypeDomaineId = table.Column<int>(nullable: true),
                    TypeMoyenId = table.Column<int>(nullable: true),
                    DateDernierContact = table.Column<DateTime>(nullable: true),
                    CreateurId = table.Column<string>(nullable: true),
                    FormateurIdDernierContact = table.Column<string>(nullable: true),
                    StagiaireIdDernierContact = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entreprises", x => x.EntrepriseId);
                    table.ForeignKey(
                        name: "FK_Entreprises_AspNetUsers_CreateurId",
                        column: x => x.CreateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entreprises_AspNetUsers_FormateurIdDernierContact",
                        column: x => x.FormateurIdDernierContact,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entreprises_AspNetUsers_StagiaireIdDernierContact",
                        column: x => x.StagiaireIdDernierContact,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entreprises_TypeDomaines_TypeDomaineId",
                        column: x => x.TypeDomaineId,
                        principalTable: "TypeDomaines",
                        principalColumn: "TypeDomaineId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entreprises_TypeEntreprises_TypeEntrepriseId",
                        column: x => x.TypeEntrepriseId,
                        principalTable: "TypeEntreprises",
                        principalColumn: "TypeEntrepriseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entreprises_TypeMoyens_TypeMoyenId",
                        column: x => x.TypeMoyenId,
                        principalTable: "TypeMoyens",
                        principalColumn: "TypeMoyenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    TelFix = table.Column<string>(maxLength: 13, nullable: true),
                    Natel = table.Column<string>(maxLength: 13, nullable: true),
                    Fax = table.Column<string>(maxLength: 13, nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: true),
                    DateModification = table.Column<DateTime>(nullable: true),
                    CreateurId = table.Column<string>(nullable: true),
                    ModificateurId = table.Column<string>(nullable: true),
                    EntrepriseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_CreateurId",
                        column: x => x.CreateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contacts_Entreprises_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprises",
                        principalColumn: "EntrepriseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_ModificateurId",
                        column: x => x.ModificateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntrepriseMetiers",
                columns: table => new
                {
                    EntrepriseId = table.Column<int>(nullable: false),
                    TypeMetierId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrepriseMetiers", x => new { x.EntrepriseId, x.TypeMetierId });
                    table.ForeignKey(
                        name: "FK_EntrepriseMetiers_Entreprises_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprises",
                        principalColumn: "EntrepriseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrepriseMetiers_TypeMetiers_TypeMetierId",
                        column: x => x.TypeMetierId,
                        principalTable: "TypeMetiers",
                        principalColumn: "TypeMetierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntrepriseOffres",
                columns: table => new
                {
                    EntrepriseId = table.Column<int>(nullable: false),
                    TypeOffreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrepriseOffres", x => new { x.EntrepriseId, x.TypeOffreId });
                    table.ForeignKey(
                        name: "FK_EntrepriseOffres_Entreprises_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprises",
                        principalColumn: "EntrepriseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrepriseOffres_TypeOffres_TypeOffreId",
                        column: x => x.TypeOffreId,
                        principalTable: "TypeOffres",
                        principalColumn: "TypeOffreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    StageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(maxLength: 30, nullable: false),
                    Debut = table.Column<DateTime>(nullable: false),
                    Fin = table.Column<DateTime>(nullable: true),
                    Bilan = table.Column<string>(nullable: true),
                    ActionSuivi = table.Column<string>(maxLength: 500, nullable: true),
                    Remarque = table.Column<string>(maxLength: 500, nullable: true),
                    Rapport = table.Column<bool>(nullable: true),
                    Attestation = table.Column<bool>(nullable: true),
                    Repas = table.Column<bool>(nullable: true),
                    Trajets = table.Column<bool>(nullable: true),
                    StagiaireId = table.Column<string>(nullable: false),
                    EntrepriseId = table.Column<int>(nullable: false),
                    CreateurId = table.Column<string>(nullable: false),
                    TypeMetierId = table.Column<int>(nullable: false),
                    TypeStageId = table.Column<int>(nullable: false),
                    HoraireMatin = table.Column<string>(maxLength: 11, nullable: true),
                    HoraireApresMidi = table.Column<string>(maxLength: 11, nullable: true),
                    TypeAnnonceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.StageId);
                    table.ForeignKey(
                        name: "FK_Stages_AspNetUsers_CreateurId",
                        column: x => x.CreateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_Entreprises_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprises",
                        principalColumn: "EntrepriseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_AspNetUsers_StagiaireId",
                        column: x => x.StagiaireId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_TypeAnnonces_TypeAnnonceId",
                        column: x => x.TypeAnnonceId,
                        principalTable: "TypeAnnonces",
                        principalColumn: "TypeAnnonceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_TypeMetiers_TypeMetierId",
                        column: x => x.TypeMetierId,
                        principalTable: "TypeMetiers",
                        principalColumn: "TypeMetierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stages_TypeStages_TypeStageId",
                        column: x => x.TypeStageId,
                        principalTable: "TypeStages",
                        principalColumn: "TypeStageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TypeAffiliationId",
                table: "AspNetUsers",
                column: "TypeAffiliationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CreateurId",
                table: "Contacts",
                column: "CreateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EntrepriseId",
                table: "Contacts",
                column: "EntrepriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ModificateurId",
                table: "Contacts",
                column: "ModificateurId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrepriseMetiers_TypeMetierId",
                table: "EntrepriseMetiers",
                column: "TypeMetierId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrepriseOffres_TypeOffreId",
                table: "EntrepriseOffres",
                column: "TypeOffreId");

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_CreateurId",
                table: "Entreprises",
                column: "CreateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_FormateurIdDernierContact",
                table: "Entreprises",
                column: "FormateurIdDernierContact");

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_StagiaireIdDernierContact",
                table: "Entreprises",
                column: "StagiaireIdDernierContact");

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_TypeDomaineId",
                table: "Entreprises",
                column: "TypeDomaineId");

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_TypeEntrepriseId",
                table: "Entreprises",
                column: "TypeEntrepriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Entreprises_TypeMoyenId",
                table: "Entreprises",
                column: "TypeMoyenId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_CreateurId",
                table: "Stages",
                column: "CreateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_EntrepriseId",
                table: "Stages",
                column: "EntrepriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_StagiaireId",
                table: "Stages",
                column: "StagiaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_TypeAnnonceId",
                table: "Stages",
                column: "TypeAnnonceId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_TypeMetierId",
                table: "Stages",
                column: "TypeMetierId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_TypeStageId",
                table: "Stages",
                column: "TypeStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TypeAffiliations_TypeAffiliationId",
                table: "AspNetUsers",
                column: "TypeAffiliationId",
                principalTable: "TypeAffiliations",
                principalColumn: "TypeAffiliationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TypeAffiliations_TypeAffiliationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "EntrepriseMetiers");

            migrationBuilder.DropTable(
                name: "EntrepriseOffres");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "TypeAffiliations");

            migrationBuilder.DropTable(
                name: "TypeOffres");

            migrationBuilder.DropTable(
                name: "Entreprises");

            migrationBuilder.DropTable(
                name: "TypeAnnonces");

            migrationBuilder.DropTable(
                name: "TypeMetiers");

            migrationBuilder.DropTable(
                name: "TypeStages");

            migrationBuilder.DropTable(
                name: "TypeDomaines");

            migrationBuilder.DropTable(
                name: "TypeEntreprises");

            migrationBuilder.DropTable(
                name: "TypeMoyens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TypeAffiliationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TypeAffiliationId",
                table: "AspNetUsers");
        }
    }
}
