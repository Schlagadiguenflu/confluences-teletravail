using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class changeTypeMetier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TypeMetiers_Code",
                table: "TypeMetiers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "TypeMetiers");

            migrationBuilder.AddColumn<string>(
                name: "OldNames",
                table: "TypeMetiers",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldNames",
                table: "TypeMetiers");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TypeMetiers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeMetiers_Code",
                table: "TypeMetiers",
                column: "Code",
                unique: true);
        }
    }
}
