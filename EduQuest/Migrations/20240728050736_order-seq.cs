using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest.Migrations
{
    /// <inheritdoc />
    public partial class orderseq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Contents");

            migrationBuilder.CreateSequence<int>(
                name: "ContentOrders");

            migrationBuilder.CreateSequence<int>(
                name: "SectionOrders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR SectionOrders",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Contents",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR ContentOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Contents");

            migrationBuilder.DropSequence(
                name: "ContentOrders");

            migrationBuilder.DropSequence(
                name: "SectionOrders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Sections",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR SectionOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
