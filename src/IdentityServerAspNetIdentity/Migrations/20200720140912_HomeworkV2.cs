using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class HomeworkV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeworkV2s",
                columns: table => new
                {
                    HomeworkV2Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeworkV2Date = table.Column<DateTime>(nullable: false),
                    HomeworkV2Name = table.Column<string>(nullable: false),
                    HomeworkTypeId = table.Column<int>(nullable: false),
                    SessionId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkV2s", x => x.HomeworkV2Id);
                    table.ForeignKey(
                        name: "FK_HomeworkV2s_HomeworkTypes_HomeworkTypeId",
                        column: x => x.HomeworkTypeId,
                        principalTable: "HomeworkTypes",
                        principalColumn: "HomeworkTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeworkV2s_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeworkV2s_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkV2s_HomeworkTypeId",
                table: "HomeworkV2s",
                column: "HomeworkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkV2s_SessionId",
                table: "HomeworkV2s",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkV2s_TeacherId",
                table: "HomeworkV2s",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkV2s");
        }
    }
}
