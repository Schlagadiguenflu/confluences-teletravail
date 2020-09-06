using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class nullableOnStageentrepriseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Entreprises_EntrepriseId",
                table: "Stages");

            migrationBuilder.AlterColumn<int>(
                name: "EntrepriseId",
                table: "Stages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Entreprises_EntrepriseId",
                table: "Stages",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "EntrepriseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Entreprises_EntrepriseId",
                table: "Stages");

            migrationBuilder.AlterColumn<int>(
                name: "EntrepriseId",
                table: "Stages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Entreprises_EntrepriseId",
                table: "Stages",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "EntrepriseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
