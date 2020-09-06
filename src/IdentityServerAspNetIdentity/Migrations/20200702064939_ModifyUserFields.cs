using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class ModifyUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
