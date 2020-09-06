using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class orderHomeworkType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeworkOrder",
                table: "HomeworkTypes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeworkOrder",
                table: "HomeworkTypes");
        }
    }
}
