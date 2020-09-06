using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class sessionnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionNumberId",
                table: "Sessions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SessionNumbers",
                columns: table => new
                {
                    SessionNumberId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionNumbers", x => x.SessionNumberId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SessionNumberId",
                table: "Sessions",
                column: "SessionNumberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_SessionNumbers_SessionNumberId",
                table: "Sessions",
                column: "SessionNumberId",
                principalTable: "SessionNumbers",
                principalColumn: "SessionNumberId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_SessionNumbers_SessionNumberId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionNumbers");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_SessionNumberId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SessionNumberId",
                table: "Sessions");
        }
    }
}
