using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class primarykeyintonAffiliation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeAffiliationId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeAffiliations",
                columns: table => new
                {
                    TypeAffiliationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Libelle = table.Column<string>(maxLength: 50, nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
