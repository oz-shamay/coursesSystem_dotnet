using Microsoft.EntityFrameworkCore.Migrations;

namespace coursesSystem.Migrations
{
    public partial class removeAppuserFromAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AppUser_AppUserId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_AppUserId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Attendances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AppUserId",
                table: "Attendances",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AppUser_AppUserId",
                table: "Attendances",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
