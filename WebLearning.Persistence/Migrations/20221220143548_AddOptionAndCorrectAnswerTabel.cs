using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddOptionAndCorrectAnswerTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "OptA",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "OptB",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "OptC",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "OptD",
                table: "QuestionMonthly");

            migrationBuilder.DropColumn(
                name: "OptA",
                table: "QuestionFinal");

            migrationBuilder.DropColumn(
                name: "OptB",
                table: "QuestionFinal");

            migrationBuilder.DropColumn(
                name: "OptC",
                table: "QuestionFinal");

            migrationBuilder.DropColumn(
                name: "OptD",
                table: "QuestionFinal");

            migrationBuilder.AddColumn<int>(
                name: "CheckBoxId",
                table: "AnswerMonthly",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "AnswerMonthly",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CheckBoxId",
                table: "AnswerCourse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "AnswerCourse",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CorrectAnswerCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswerCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswerCourse_QuestionFinal_QuestionFinalId",
                        column: x => x.QuestionFinalId,
                        principalTable: "QuestionFinal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CorrectAnswerMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswerMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswerMonthly_QuestionMonthly_QuestionMonthlyId",
                        column: x => x.QuestionMonthlyId,
                        principalTable: "QuestionMonthly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionCourse_QuestionFinal_QuestionFinalId",
                        column: x => x.QuestionFinalId,
                        principalTable: "QuestionFinal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OptionMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionMonthly_QuestionMonthly_QuestionMonthlyId",
                        column: x => x.QuestionMonthlyId,
                        principalTable: "QuestionMonthly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswerCourse_QuestionFinalId",
                table: "CorrectAnswerCourse",
                column: "QuestionFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswerMonthly_QuestionMonthlyId",
                table: "CorrectAnswerMonthly",
                column: "QuestionMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionCourse_QuestionFinalId",
                table: "OptionCourse",
                column: "QuestionFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionMonthly_QuestionMonthlyId",
                table: "OptionMonthly",
                column: "QuestionMonthlyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectAnswerCourse");

            migrationBuilder.DropTable(
                name: "CorrectAnswerMonthly");

            migrationBuilder.DropTable(
                name: "OptionCourse");

            migrationBuilder.DropTable(
                name: "OptionMonthly");

            migrationBuilder.DropColumn(
                name: "CheckBoxId",
                table: "AnswerMonthly");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "AnswerMonthly");

            migrationBuilder.DropColumn(
                name: "CheckBoxId",
                table: "AnswerCourse");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "AnswerCourse");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptA",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptB",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptC",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptD",
                table: "QuestionMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptA",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptB",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptC",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptD",
                table: "QuestionFinal",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
