using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddAnswerMonthly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerMonthly_QuestionMonthly_QuestionMonthlyId",
                        column: x => x.QuestionMonthlyId,
                        principalTable: "QuestionMonthly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerMonthly_QuestionMonthlyId",
                table: "AnswerMonthly",
                column: "QuestionMonthlyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerMonthly");
        }
    }
}
