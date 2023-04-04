using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class ChangeToListAlertInCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alert_CourseId",
                table: "Alert");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CourseId",
                table: "Alert",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alert_CourseId",
                table: "Alert");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CourseId",
                table: "Alert",
                column: "CourseId",
                unique: true);
        }
    }
}
