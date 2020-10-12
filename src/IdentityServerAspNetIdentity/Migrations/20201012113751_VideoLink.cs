using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class VideoLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "Theories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "ExercicesAlone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "Exercices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "Theories");

            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "ExercicesAlone");

            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "Exercices");
        }
    }
}
