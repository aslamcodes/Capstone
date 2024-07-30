using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest.Migrations
{
    /// <inheritdoc />
    public partial class objetive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseObjective",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseObjective",
                table: "Courses");
        }
    }
}
