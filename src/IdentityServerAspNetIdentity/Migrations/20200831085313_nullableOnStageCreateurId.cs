using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class nullableOnStageCreateurId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_AspNetUsers_CreateurId",
                table: "Stages");

            migrationBuilder.AlterColumn<string>(
                name: "CreateurId",
                table: "Stages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_AspNetUsers_CreateurId",
                table: "Stages",
                column: "CreateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_AspNetUsers_CreateurId",
                table: "Stages");

            migrationBuilder.AlterColumn<string>(
                name: "CreateurId",
                table: "Stages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_AspNetUsers_CreateurId",
                table: "Stages",
                column: "CreateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
