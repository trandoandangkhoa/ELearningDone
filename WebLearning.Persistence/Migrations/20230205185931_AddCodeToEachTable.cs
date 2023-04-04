using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddCodeToEachTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Role",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuizMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuizLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuizCourse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OtherFileUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OptionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OptionLessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OptionCourse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "LessionVideoImage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Lession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CourseRole",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuizMonthly");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuizLession");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuizCourse");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuestionLession");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuestionFinal");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "OtherFileUpload");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "OptionMonthly");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "OptionLessions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "OptionCourse");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "LessionVideoImage");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Lession");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CourseRole");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Account");
        }
    }
}
