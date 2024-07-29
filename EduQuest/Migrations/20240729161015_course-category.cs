using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduQuest.Migrations
{
    /// <inheritdoc />
    public partial class coursecategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseCategoryId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CourseCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Courses on various programming languages and software development techniques.", "Programming" },
                    { 2, "Courses on graphic design, UX/UI, and other design disciplines.", "Design" },
                    { 3, "Courses covering business management, entrepreneurship, and corporate strategy.", "Business" },
                    { 4, "Courses on digital marketing, advertising, and sales strategies.", "Marketing" },
                    { 5, "Courses on music theory, instrument training, and music production.", "Music" },
                    { 6, "Courses on photography techniques, camera handling, and photo editing.", "Photography" },
                    { 7, "Courses on physical health, fitness routines, and nutrition.", "Health & Fitness" },
                    { 8, "Courses focused on personal growth, self-improvement, and life skills.", "Personal Development" },
                    { 9, "Courses covering lifestyle improvements, hobbies, and general well-being.", "Lifestyle" },
                    { 10, "Courses on IT infrastructure, software applications, and tech support.", "IT & Software" },
                    { 11, "Courses on learning new languages and improving language proficiency.", "Language" },
                    { 12, "Courses covering academic subjects and school-level education.", "Academics" },
                    { 15, "Courses on various engineering disciplines and technical skills.", "Engineering" },
                    { 16, "Courses covering different scientific fields and research methods.", "Science" },
                    { 17, "Courses on mathematics, from basic arithmetic to advanced calculus.", "Mathematics" },
                    { 20, "Courses on data analysis, machine learning, and big data.", "Data Science" },
                    { 21, "Courses on various forms of art, history, and cultural studies.", "Art & Culture" },
                    { 22, "Courses on financial management, accounting principles, and investments.", "Finance & Accounting" },
                    { 24, "Courses on sales techniques, customer relations, and sales management.", "Sales" },
                    { 26, "Courses on management skills, leadership, and organizational behavior.", "Management" },
                    { 27, "Courses on effective communication, public speaking, and interpersonal skills.", "Communication" },
                    { 42, "Courses on physical fitness, exercise routines, and healthy living.", "Fitness" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoryId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseCategories");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseCategoryId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseCategoryId",
                table: "Courses");
        }
    }
}
