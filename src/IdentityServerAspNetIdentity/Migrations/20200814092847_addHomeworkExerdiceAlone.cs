using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class addHomeworkExerdiceAlone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeworkV2StudentExerciceAlones",
                columns: table => new
                {
                    HomeworkV2StudentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeworkStudentDate = table.Column<DateTime>(nullable: false),
                    HomeworkFile = table.Column<string>(nullable: false),
                    HomeworkFileTeacher = table.Column<string>(nullable: true),
                    HomeworkCommentaryTeacher = table.Column<string>(nullable: true),
                    ExerciceId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkV2StudentExerciceAlones", x => x.HomeworkV2StudentId);
                    table.ForeignKey(
                        name: "FK_HomeworkV2StudentExerciceAlones_ExercicesAlone_ExerciceId",
                        column: x => x.ExerciceId,
                        principalTable: "ExercicesAlone",
                        principalColumn: "ExerciceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeworkV2StudentExerciceAlones_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkV2StudentExerciceAlones_ExerciceId",
                table: "HomeworkV2StudentExerciceAlones",
                column: "ExerciceId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkV2StudentExerciceAlones_StudentId",
                table: "HomeworkV2StudentExerciceAlones",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkV2StudentExerciceAlones");
        }
    }
}
