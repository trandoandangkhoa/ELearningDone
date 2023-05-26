using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class RemoveQuestionIdInHistorySubmit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QuestionFinal");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "HistorySubmitMonthly");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "HistorySubmitLession");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "HistorySubmitFinal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "HistorySubmitMonthly",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "HistorySubmitLession",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "HistorySubmitFinal",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
