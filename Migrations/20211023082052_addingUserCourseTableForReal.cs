using Microsoft.EntityFrameworkCore.Migrations;

namespace coursesSystem.Migrations
{
    public partial class addingUserCourseTableForReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserCourse_AppUser_AppUserId",
                table: "AppUserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserCourse_Courses_CourseId",
                table: "AppUserCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserCourse",
                table: "AppUserCourse");

            migrationBuilder.RenameTable(
                name: "AppUserCourse",
                newName: "UserCourses");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserCourse_CourseId",
                table: "UserCourses",
                newName: "IX_UserCourses_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses",
                columns: new[] { "AppUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_AppUser_AppUserId",
                table: "UserCourses",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Courses_CourseId",
                table: "UserCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "courseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_AppUser_AppUserId",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Courses_CourseId",
                table: "UserCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses");

            migrationBuilder.RenameTable(
                name: "UserCourses",
                newName: "AppUserCourse");

            migrationBuilder.RenameIndex(
                name: "IX_UserCourses_CourseId",
                table: "AppUserCourse",
                newName: "IX_AppUserCourse_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserCourse",
                table: "AppUserCourse",
                columns: new[] { "AppUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserCourse_AppUser_AppUserId",
                table: "AppUserCourse",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserCourse_Courses_CourseId",
                table: "AppUserCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "courseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
