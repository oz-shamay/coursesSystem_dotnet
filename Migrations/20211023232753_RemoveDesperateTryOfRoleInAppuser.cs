using Microsoft.EntityFrameworkCore.Migrations;

namespace coursesSystem.Migrations
{
    public partial class RemoveDesperateTryOfRoleInAppuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Roles_IdentityRoleId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_IdentityRoleId",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "IdentityRoleId",
                table: "AppUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityRoleId",
                table: "AppUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_IdentityRoleId",
                table: "AppUser",
                column: "IdentityRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Roles_IdentityRoleId",
                table: "AppUser",
                column: "IdentityRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
