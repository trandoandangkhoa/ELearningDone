using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddMonthlytest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionLession_Quiz_QuizLessionId",
                table: "QuestionLession");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_HistorySubmitLession_HistorySubmitLessionId",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Lession_LessionId",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_ReportUserScore_ReportUserScoreId",
                table: "Quiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz");

            migrationBuilder.RenameTable(
                name: "Quiz",
                newName: "QuizLession");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_ReportUserScoreId",
                table: "QuizLession",
                newName: "IX_QuizLession_ReportUserScoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_LessionId",
                table: "QuizLession",
                newName: "IX_QuizLession_LessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_HistorySubmitLessionId",
                table: "QuizLession",
                newName: "IX_QuizLession_HistorySubmitLessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizLession",
                table: "QuizLession",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "HistorySubmitMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCompoleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Submit = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySubmitMonthly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserScoreMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserScoreMonthly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizMonthly",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToDo = table.Column<int>(type: "int", nullable: false),
                    ScorePass = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    HistorySubmitMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportUserScoreMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizMonthly", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuizMonthly_HistorySubmitMonthly_HistorySubmitMonthlyId",
                        column: x => x.HistorySubmitMonthlyId,
                        principalTable: "HistorySubmitMonthly",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizMonthly_ReportUserScoreMonthly_ReportUserScoreMonthlyId",
                        column: x => x.ReportUserScoreMonthlyId,
                        principalTable: "ReportUserScoreMonthly",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizMonthly_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionMonthly_QuizMonthly_QuizMonthlyId",
                        column: x => x.QuizMonthlyId,
                        principalTable: "QuizMonthly",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMonthly_QuizMonthlyId",
                table: "QuestionMonthly",
                column: "QuizMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizMonthly_HistorySubmitMonthlyId",
                table: "QuizMonthly",
                column: "HistorySubmitMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizMonthly_ReportUserScoreMonthlyId",
                table: "QuizMonthly",
                column: "ReportUserScoreMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizMonthly_RoleId",
                table: "QuizMonthly",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionLession_QuizLession_QuizLessionId",
                table: "QuestionLession",
                column: "QuizLessionId",
                principalTable: "QuizLession",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizLession_HistorySubmitLession_HistorySubmitLessionId",
                table: "QuizLession",
                column: "HistorySubmitLessionId",
                principalTable: "HistorySubmitLession",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizLession_Lession_LessionId",
                table: "QuizLession",
                column: "LessionId",
                principalTable: "Lession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizLession_ReportUserScore_ReportUserScoreId",
                table: "QuizLession",
                column: "ReportUserScoreId",
                principalTable: "ReportUserScore",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionLession_QuizLession_QuizLessionId",
                table: "QuestionLession");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizLession_HistorySubmitLession_HistorySubmitLessionId",
                table: "QuizLession");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizLession_Lession_LessionId",
                table: "QuizLession");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizLession_ReportUserScore_ReportUserScoreId",
                table: "QuizLession");

            migrationBuilder.DropTable(
                name: "QuestionMonthly");

            migrationBuilder.DropTable(
                name: "QuizMonthly");

            migrationBuilder.DropTable(
                name: "HistorySubmitMonthly");

            migrationBuilder.DropTable(
                name: "ReportUserScoreMonthly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizLession",
                table: "QuizLession");

            migrationBuilder.RenameTable(
                name: "QuizLession",
                newName: "Quiz");

            migrationBuilder.RenameIndex(
                name: "IX_QuizLession_ReportUserScoreId",
                table: "Quiz",
                newName: "IX_Quiz_ReportUserScoreId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizLession_LessionId",
                table: "Quiz",
                newName: "IX_Quiz_LessionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizLession_HistorySubmitLessionId",
                table: "Quiz",
                newName: "IX_Quiz_HistorySubmitLessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionLession_Quiz_QuizLessionId",
                table: "QuestionLession",
                column: "QuizLessionId",
                principalTable: "Quiz",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_HistorySubmitLession_HistorySubmitLessionId",
                table: "Quiz",
                column: "HistorySubmitLessionId",
                principalTable: "HistorySubmitLession",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Lession_LessionId",
                table: "Quiz",
                column: "LessionId",
                principalTable: "Lession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_ReportUserScore_ReportUserScoreId",
                table: "Quiz",
                column: "ReportUserScoreId",
                principalTable: "ReportUserScore",
                principalColumn: "Id");
        }
    }
}
