using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class ChangeQuestionCourseToQuestionFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionCourse_QuestionFinal_QuestionFinalId",
                table: "OptionCourse");

            migrationBuilder.DropColumn(
                name: "QuestionCourseId",
                table: "OptionCourse");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionFinalId",
                table: "OptionCourse",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OptionCourse_QuestionFinal_QuestionFinalId",
                table: "OptionCourse",
                column: "QuestionFinalId",
                principalTable: "QuestionFinal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionCourse_QuestionFinal_QuestionFinalId",
                table: "OptionCourse");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionFinalId",
                table: "OptionCourse",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionCourseId",
                table: "OptionCourse",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_OptionCourse_QuestionFinal_QuestionFinalId",
                table: "OptionCourse",
                column: "QuestionFinalId",
                principalTable: "QuestionFinal",
                principalColumn: "Id");
        }
    }
}
