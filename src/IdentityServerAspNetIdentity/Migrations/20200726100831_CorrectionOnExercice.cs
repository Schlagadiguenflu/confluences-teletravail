using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class CorrectionOnExercice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CorrectionDate",
                table: "Exercices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CorrectionLink",
                table: "Exercices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectionDate",
                table: "Exercices");

            migrationBuilder.DropColumn(
                name: "CorrectionLink",
                table: "Exercices");
        }
    }
}
