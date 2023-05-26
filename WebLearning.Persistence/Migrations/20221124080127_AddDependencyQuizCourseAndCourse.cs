using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddDependencyQuizCourseAndCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuizCourse_CourseId",
                table: "QuizCourse");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_CourseId",
                table: "QuizCourse",
                column: "CourseId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuizCourse_CourseId",
                table: "QuizCourse");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_CourseId",
                table: "QuizCourse",
                column: "CourseId");
        }
    }
}
