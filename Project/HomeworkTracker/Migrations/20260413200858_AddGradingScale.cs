using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeworkTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddGradingScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradingScale",
                table: "Assignments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradingScale",
                table: "Assignments");
        }
    }
}
