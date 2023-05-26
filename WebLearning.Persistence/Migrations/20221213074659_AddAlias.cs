using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddAlias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Role",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "QuizMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "QuizLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "QuizCourse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "QuizMonthly");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "QuizLession");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "QuizCourse");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "QuestionLession");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "QuestionFinal");
        }
    }
}
