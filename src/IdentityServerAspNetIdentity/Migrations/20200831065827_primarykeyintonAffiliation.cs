using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class primarykeyintonAffiliation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TypeAffiliations_TypeAffiliationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TypeAffiliations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TypeAffiliationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TypeAffiliationId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeAffiliationId",
                table: "AspNetUsers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeAffiliations",
                columns: table => new
                {
                    TypeAffiliationId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Libelle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAffiliations", x => x.TypeAffiliationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TypeAffiliationId",
                table: "AspNetUsers",
                column: "TypeAffiliationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TypeAffiliations_TypeAffiliationId",
                table: "AspNetUsers",
                column: "TypeAffiliationId",
                principalTable: "TypeAffiliations",
                principalColumn: "TypeAffiliationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
