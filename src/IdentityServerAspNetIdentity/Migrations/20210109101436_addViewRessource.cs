using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace IdentityServerAspNetIdentity.Migrations
{
    public partial class addViewRessource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var rootPath = AppContext.BaseDirectory;
            rootPath = rootPath.Substring(0, rootPath.Length - 25);
            var sqlFile = Path.Combine(rootPath,"Migrations","SQL", @"RessourceView.sql");

            if (File.Exists(sqlFile))
            {
                migrationBuilder.Sql(File.ReadAllText(sqlFile));
            }
            else
            {
                throw new Exception($"Migration .sql file not found: ${sqlFile}");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
