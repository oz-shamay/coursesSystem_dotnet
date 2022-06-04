using Microsoft.EntityFrameworkCore.Migrations;

namespace coursesSystem.Migrations
{
    public partial class addingStydentsToCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "courseId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_courseId",
                table: "AspNetUsers",
                column: "courseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_courseId",
                table: "AspNetUsers",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "courseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_courseId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_courseId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "courseId",
                table: "AspNetUsers");
        }
    }
}
