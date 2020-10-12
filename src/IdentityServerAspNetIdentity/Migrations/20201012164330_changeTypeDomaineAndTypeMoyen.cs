using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class changeTypeDomaineAndTypeMoyen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TypeMoyens_Code",
                table: "TypeMoyens");

            migrationBuilder.DropIndex(
                name: "IX_TypeDomaines_Code",
                table: "TypeDomaines");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "TypeMoyens");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "TypeDomaines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TypeMoyens",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TypeDomaines",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeMoyens_Code",
                table: "TypeMoyens",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeDomaines_Code",
                table: "TypeDomaines",
                column: "Code",
                unique: true);
        }
    }
}
