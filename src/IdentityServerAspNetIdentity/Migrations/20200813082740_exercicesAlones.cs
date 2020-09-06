using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class exercicesAlones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExercicesAlone",
                columns: table => new
                {
                    ExerciceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciceDate = table.Column<DateTime>(nullable: false),
                    ExerciceName = table.Column<string>(nullable: true),
                    ExerciceLink = table.Column<string>(nullable: true),
                    AudioLink = table.Column<string>(nullable: true),
                    CorrectionDate = table.Column<DateTime>(nullable: false),
                    CorrectionLink = table.Column<string>(nullable: true),
                    IsHomeworkAdditionnal = table.Column<bool>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false),
                    HomeworkV2Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercicesAlone", x => x.ExerciceId);
                    table.ForeignKey(
                        name: "FK_ExercicesAlone_HomeworkV2s_HomeworkV2Id",
                        column: x => x.HomeworkV2Id,
                        principalTable: "HomeworkV2s",
                        principalColumn: "HomeworkV2Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExercicesAlone_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercicesAlone_HomeworkV2Id",
                table: "ExercicesAlone",
                column: "HomeworkV2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExercicesAlone_TeacherId",
                table: "ExercicesAlone",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercicesAlone");
        }
    }
}
