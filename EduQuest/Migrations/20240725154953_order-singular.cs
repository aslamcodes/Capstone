using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest.Migrations
{
    /// <inheritdoc />
    public partial class ordersingular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseOrder");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "OrderedCourseId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedCourseId",
                table: "Orders",
                column: "OrderedCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Courses_OrderedCourseId",
                table: "Orders",
                column: "OrderedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Courses_OrderedCourseId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderedCourseId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderedCourseId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.CreateTable(
                name: "CourseOrder",
                columns: table => new
                {
                    OrderedCoursesId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseOrder", x => new { x.OrderedCoursesId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_CourseOrder_Courses_OrderedCoursesId",
                        column: x => x.OrderedCoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseOrder_OrdersId",
                table: "CourseOrder",
                column: "OrdersId");
        }
    }
}
