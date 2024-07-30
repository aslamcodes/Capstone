using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduQuest.Migrations
{
    /// <inheritdoc />
    public partial class courseskill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSkills_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "HTML" },
                    { 2, "CSS" },
                    { 3, "JavaScript" },
                    { 4, "Python" },
                    { 5, "Java" },
                    { 6, "C#" },
                    { 7, "C++" },
                    { 8, "Ruby" },
                    { 9, "PHP" },
                    { 10, "SQL" },
                    { 11, "React" },
                    { 12, "Angular" },
                    { 13, "Vue" },
                    { 14, "Node.js" },
                    { 15, "Express" },
                    { 16, "Django" },
                    { 17, "Flask" },
                    { 18, "Spring" },
                    { 19, "Swift" },
                    { 20, "Kotlin" },
                    { 21, "TypeScript" },
                    { 22, "Go" },
                    { 23, "Rust" },
                    { 24, "Perl" },
                    { 25, "Scala" },
                    { 26, "Groovy" },
                    { 27, "Haskell" },
                    { 28, "MATLAB" },
                    { 29, "R" },
                    { 30, "SAS" },
                    { 31, "Assembly" },
                    { 32, "Shell Scripting" },
                    { 33, "Objective-C" },
                    { 34, "F#" },
                    { 35, "Elixir" },
                    { 36, "Clojure" },
                    { 37, "Erlang" },
                    { 38, "Dart" },
                    { 39, "Julia" },
                    { 40, "GraphQL" },
                    { 41, "Docker" },
                    { 42, "Kubernetes" },
                    { 43, "Terraform" },
                    { 44, "Ansible" },
                    { 45, "Puppet" },
                    { 46, "Chef" },
                    { 47, "Jenkins" },
                    { 48, "Travis CI" },
                    { 49, "CircleCI" },
                    { 50, "Git" },
                    { 51, "SVN" },
                    { 52, "Mercurial" },
                    { 53, "Public Speaking" },
                    { 54, "Creative Writing" },
                    { 55, "Project Management" },
                    { 56, "Graphic Design" },
                    { 57, "Cooking" },
                    { 58, "Photography" },
                    { 59, "Yoga" },
                    { 60, "Meditation" },
                    { 61, "Gardening" },
                    { 62, "Baking" },
                    { 63, "Event Planning" },
                    { 64, "First Aid" },
                    { 65, "Leadership" },
                    { 66, "Teamwork" },
                    { 67, "Negotiation" },
                    { 68, "Time Management" },
                    { 69, "Critical Thinking" },
                    { 70, "Problem Solving" },
                    { 71, "Customer Service" },
                    { 72, "Foreign Languages" },
                    { 73, "Musical Instrument" },
                    { 74, "Dance" },
                    { 75, "Fitness Training" },
                    { 76, "Interior Design" },
                    { 77, "Fashion Design" },
                    { 78, "Makeup Artistry" },
                    { 79, "Sewing" },
                    { 80, "Knitting" },
                    { 81, "Public Relations" },
                    { 82, "Marketing" },
                    { 83, "Sales" },
                    { 84, "Finance" },
                    { 85, "Accounting" },
                    { 86, "Investing" },
                    { 87, "Real Estate" },
                    { 88, "Economics" },
                    { 89, "Law" },
                    { 90, "History" },
                    { 91, "Philosophy" },
                    { 92, "Psychology" },
                    { 93, "Sociology" },
                    { 94, "Anthropology" },
                    { 95, "Political Science" },
                    { 96, "Geography" },
                    { 97, "Journalism" },
                    { 98, "Film Production" },
                    { 99, "Screenwriting" },
                    { 100, "Acting" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSkills_CourseId",
                table: "CourseSkills",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSkills_SkillId",
                table: "CourseSkills",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseSkills");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
