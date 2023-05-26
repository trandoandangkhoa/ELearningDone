using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddNewMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "New",
                table: "QuizMonthly",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "New",
                table: "QuizLession",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "New",
                table: "QuizCourse",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "New",
                table: "LessionVideoImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "New",
                table: "Lession",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "New",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "New",
                table: "QuizMonthly");

            migrationBuilder.DropColumn(
                name: "New",
                table: "QuizLession");

            migrationBuilder.DropColumn(
                name: "New",
                table: "QuizCourse");

            migrationBuilder.DropColumn(
                name: "New",
                table: "LessionVideoImage");

            migrationBuilder.DropColumn(
                name: "New",
                table: "Lession");

            migrationBuilder.DropColumn(
                name: "New",
                table: "Course");
        }
    }
}
