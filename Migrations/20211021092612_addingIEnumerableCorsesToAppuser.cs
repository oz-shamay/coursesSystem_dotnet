using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace coursesSystem.Migrations
{
    public partial class addingIEnumerableCorsesToAppuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "userId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppUserCourse",
                columns: table => new
                {
                    coursesListcourseId = table.Column<int>(type: "int", nullable: false),
                    studentsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserCourse", x => new { x.coursesListcourseId, x.studentsId });
                    table.ForeignKey(
                        name: "FK_AppUserCourse_AspNetUsers_studentsId",
                        column: x => x.studentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserCourse_Courses_coursesListcourseId",
                        column: x => x.coursesListcourseId,
                        principalTable: "Courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserCourse_studentsId",
                table: "AppUserCourse",
                column: "studentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserCourse");

            migrationBuilder.AddColumn<int>(
                name: "courseId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "userId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
