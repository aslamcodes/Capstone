using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest.Migrations
{
    /// <inheritdoc />
    public partial class coursethumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseThumbnailPicture",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseThumbnailPicture",
                table: "Courses");
        }
    }
}
