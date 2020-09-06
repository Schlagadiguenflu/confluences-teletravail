using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class HomeworksTheoryExercices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Theories",
                columns: table => new
                {
                    TheoryId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TheoryDate = table.Column<DateTime>(nullable: false),
                    TheoryName = table.Column<string>(nullable: true),
                    TheoryLink = table.Column<string>(nullable: true),
                    TeacherId = table.Column<string>(nullable: false),
                    HomeworkV2Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theories", x => x.TheoryId);
                    table.ForeignKey(
                        name: "FK_Theories_HomeworkV2s_HomeworkV2Id",
                        column: x => x.HomeworkV2Id,
                        principalTable: "HomeworkV2s",
                        principalColumn: "HomeworkV2Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Theories_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercices",
                columns: table => new
                {
                    ExerciceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciceDate = table.Column<DateTime>(nullable: false),
                    ExerciceName = table.Column<string>(nullable: true),
                    ExerciceLink = table.Column<string>(nullable: true),
                    IsHomeworkAdditionnal = table.Column<bool>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false),
                    TheoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercices", x => x.ExerciceId);
                    table.ForeignKey(
                        name: "FK_Exercices_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercices_Theories_TheoryId",
                        column: x => x.TheoryId,
                        principalTable: "Theories",
                        principalColumn: "TheoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercices_TeacherId",
                table: "Exercices",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercices_TheoryId",
                table: "Exercices",
                column: "TheoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Theories_HomeworkV2Id",
                table: "Theories",
                column: "HomeworkV2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Theories_TeacherId",
                table: "Theories",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercices");

            migrationBuilder.DropTable(
                name: "Theories");
        }
    }
}
