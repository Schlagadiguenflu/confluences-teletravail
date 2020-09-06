using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class ModifyUserFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
