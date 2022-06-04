using Microsoft.EntityFrameworkCore.Migrations;

namespace coursesSystem.Migrations
{
    public partial class addingCoursNameToModelClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "courseName",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "courseName",
                table: "Classes");
        }
    }
}
