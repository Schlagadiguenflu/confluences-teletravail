using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnouncePages",
                columns: table => new
                {
                    AnnouncePageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnnouncePageName = table.Column<string>(nullable: false),
                    IsActivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncePages", x => x.AnnouncePageId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenderName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "HomeworkTypes",
                columns: table => new
                {
                    HomeworkTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeworkTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkTypes", x => x.HomeworkTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SchoolClassRooms",
                columns: table => new
                {
                    SchoolClassRoomId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SchoolClassRoomName = table.Column<string>(nullable: false),
                    ExplanationVideoLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClassRooms", x => x.SchoolClassRoomId);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncePageLinks",
                columns: table => new
                {
                    AnnouncePageLinkId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnnouncePageLinkName = table.Column<string>(nullable: false),
                    AnnouncePageLinkLanguage = table.Column<string>(nullable: false),
                    AnnouncePageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncePageLinks", x => x.AnnouncePageLinkId);
                    table.ForeignKey(
                        name: "FK_AnnouncePageLinks_AnnouncePages_AnnouncePageId",
                        column: x => x.AnnouncePageId,
                        principalTable: "AnnouncePages",
                        principalColumn: "AnnouncePageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Firstname = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    GenderId = table.Column<short>(nullable: false),
                    PathImage = table.Column<string>(nullable: true),
                    WantsMoreHomeworks = table.Column<bool>(nullable: false),
                    HasSeenHelpVideo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolClassRoomExplanations",
                columns: table => new
                {
                    SchoolClassRoomExplanationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageName = table.Column<string>(nullable: false),
                    AudioLink = table.Column<string>(nullable: false),
                    SchoolClassRoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClassRoomExplanations", x => x.SchoolClassRoomExplanationId);
                    table.ForeignKey(
                        name: "FK_SchoolClassRoomExplanations_SchoolClassRooms_SchoolClassRoo~",
                        column: x => x.SchoolClassRoomId,
                        principalTable: "SchoolClassRooms",
                        principalColumn: "SchoolClassRoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    SchoolClassRoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_SchoolClassRooms_SchoolClassRoomId",
                        column: x => x.SchoolClassRoomId,
                        principalTable: "SchoolClassRooms",
                        principalColumn: "SchoolClassRoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentName = table.Column<string>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Homework",
                columns: table => new
                {
                    HomeworkId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeworkDate = table.Column<DateTime>(nullable: false),
                    ClassName = table.Column<string>(nullable: true),
                    ClassLink = table.Column<string>(nullable: true),
                    ExerciceName = table.Column<string>(nullable: true),
                    ExerciceLink = table.Column<string>(nullable: true),
                    HomeworkTypeId = table.Column<int>(nullable: false),
                    SessionId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false),
                    IsHomeworkAdditionnal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homework", x => x.HomeworkId);
                    table.ForeignKey(
                        name: "FK_Homework_HomeworkTypes_HomeworkTypeId",
                        column: x => x.HomeworkTypeId,
                        principalTable: "HomeworkTypes",
                        principalColumn: "HomeworkTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Homework_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Homework_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionStudents",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionStudents", x => new { x.SessionId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_SessionStudents_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionStudents_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionTeachers",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTeachers", x => new { x.SessionId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_SessionTeachers_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionTeachers_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentStudents",
                columns: table => new
                {
                    AppointmentStudentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStudents", x => x.AppointmentStudentId);
                    table.ForeignKey(
                        name: "FK_AppointmentStudents_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentStudents_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeworkStudents",
                columns: table => new
                {
                    HomeworkStudentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeworkStudentDate = table.Column<DateTime>(nullable: false),
                    HomeworkFile = table.Column<string>(nullable: false),
                    HomeworkFileTeacher = table.Column<string>(nullable: true),
                    HomeworkId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkStudents", x => x.HomeworkStudentId);
                    table.ForeignKey(
                        name: "FK_HomeworkStudents_Homework_HomeworkId",
                        column: x => x.HomeworkId,
                        principalTable: "Homework",
                        principalColumn: "HomeworkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeworkStudents_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncePageLinks_AnnouncePageId",
                table: "AnnouncePageLinks",
                column: "AnnouncePageId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TeacherId",
                table: "Appointments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentStudents_AppointmentId",
                table: "AppointmentStudents",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentStudents_StudentId",
                table: "AppointmentStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GenderId",
                table: "AspNetUsers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homework_HomeworkTypeId",
                table: "Homework",
                column: "HomeworkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Homework_SessionId",
                table: "Homework",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Homework_TeacherId",
                table: "Homework",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkStudents_HomeworkId",
                table: "HomeworkStudents",
                column: "HomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkStudents_StudentId",
                table: "HomeworkStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClassRoomExplanations_SchoolClassRoomId",
                table: "SchoolClassRoomExplanations",
                column: "SchoolClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SchoolClassRoomId",
                table: "Sessions",
                column: "SchoolClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionStudents_StudentId",
                table: "SessionStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTeachers_TeacherId",
                table: "SessionTeachers",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncePageLinks");

            migrationBuilder.DropTable(
                name: "AppointmentStudents");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HomeworkStudents");

            migrationBuilder.DropTable(
                name: "SchoolClassRoomExplanations");

            migrationBuilder.DropTable(
                name: "SessionStudents");

            migrationBuilder.DropTable(
                name: "SessionTeachers");

            migrationBuilder.DropTable(
                name: "AnnouncePages");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Homework");

            migrationBuilder.DropTable(
                name: "HomeworkTypes");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SchoolClassRooms");

            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}
