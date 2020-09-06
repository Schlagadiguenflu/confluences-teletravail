using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class nullableOnStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_TypeAnnonces_TypeAnnonceId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_TypeStages_TypeStageId",
                table: "Stages");

            migrationBuilder.AlterColumn<int>(
                name: "TypeStageId",
                table: "Stages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "TypeAnnonceId",
                table: "Stages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_TypeAnnonces_TypeAnnonceId",
                table: "Stages",
                column: "TypeAnnonceId",
                principalTable: "TypeAnnonces",
                principalColumn: "TypeAnnonceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_TypeStages_TypeStageId",
                table: "Stages",
                column: "TypeStageId",
                principalTable: "TypeStages",
                principalColumn: "TypeStageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_TypeAnnonces_TypeAnnonceId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_TypeStages_TypeStageId",
                table: "Stages");

            migrationBuilder.AlterColumn<int>(
                name: "TypeStageId",
                table: "Stages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeAnnonceId",
                table: "Stages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_TypeAnnonces_TypeAnnonceId",
                table: "Stages",
                column: "TypeAnnonceId",
                principalTable: "TypeAnnonces",
                principalColumn: "TypeAnnonceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_TypeStages_TypeStageId",
                table: "Stages",
                column: "TypeStageId",
                principalTable: "TypeStages",
                principalColumn: "TypeStageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
