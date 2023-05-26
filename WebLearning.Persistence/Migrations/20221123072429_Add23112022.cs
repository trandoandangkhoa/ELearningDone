using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class Add23112022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistorySubmitFinal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCompoleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Submit = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySubmitFinal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorySubmitLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCompoleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Submit = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySubmitLession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserScore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserScoreFinal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserScoreFinal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHased = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorizeRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountDetail_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseImageVideo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseImageVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseImageVideo_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedTime = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lession_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherFileUpload",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherFileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherFileUpload_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizCourse",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToDo = table.Column<int>(type: "int", nullable: false),
                    ScorePass = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    HistorySubmitFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportUserScoreFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizCourse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuizCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizCourse_HistorySubmitFinal_HistorySubmitFinalId",
                        column: x => x.HistorySubmitFinalId,
                        principalTable: "HistorySubmitFinal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizCourse_ReportUserScoreFinal_ReportUserScoreFinalId",
                        column: x => x.ReportUserScoreFinalId,
                        principalTable: "ReportUserScoreFinal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessionVideoImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessionVideoImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessionVideoImage_Lession_LessionId",
                        column: x => x.LessionId,
                        principalTable: "Lession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToDo = table.Column<int>(type: "int", nullable: false),
                    ScorePass = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    HistorySubmitLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportUserScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Quiz_HistorySubmitLession_HistorySubmitLessionId",
                        column: x => x.HistorySubmitLessionId,
                        principalTable: "HistorySubmitLession",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quiz_Lession_LessionId",
                        column: x => x.LessionId,
                        principalTable: "Lession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quiz_ReportUserScore_ReportUserScoreId",
                        column: x => x.ReportUserScoreId,
                        principalTable: "ReportUserScore",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionFinal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_QuestionFinal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionFinal_QuizCourse_QuizCourseId",
                        column: x => x.QuizCourseId,
                        principalTable: "QuizCourse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_QuestionLession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLession_Quiz_QuizLessionId",
                        column: x => x.QuizLessionId,
                        principalTable: "Quiz",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerCourse_QuestionFinal_QuestionCourseId",
                        column: x => x.QuestionCourseId,
                        principalTable: "QuestionFinal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerLession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerLession_QuestionLession_QuestionLessionId",
                        column: x => x.QuestionLessionId,
                        principalTable: "QuestionLession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetail_AccountId",
                table: "AccountDetail",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerCourse_QuestionCourseId",
                table: "AnswerCourse",
                column: "QuestionCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLession_QuestionLessionId",
                table: "AnswerLession",
                column: "QuestionLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_RoleId",
                table: "Course",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseImageVideo_CourseId",
                table: "CourseImageVideo",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lession_CourseId",
                table: "Lession",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LessionVideoImage_LessionId",
                table: "LessionVideoImage",
                column: "LessionId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherFileUpload_CourseId",
                table: "OtherFileUpload",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionFinal_QuizCourseId",
                table: "QuestionFinal",
                column: "QuizCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLession_QuizLessionId",
                table: "QuestionLession",
                column: "QuizLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_HistorySubmitLessionId",
                table: "Quiz",
                column: "HistorySubmitLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_LessionId",
                table: "Quiz",
                column: "LessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_ReportUserScoreId",
                table: "Quiz",
                column: "ReportUserScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_CourseId",
                table: "QuizCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_HistorySubmitFinalId",
                table: "QuizCourse",
                column: "HistorySubmitFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_ReportUserScoreFinalId",
                table: "QuizCourse",
                column: "ReportUserScoreFinalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetail");

            migrationBuilder.DropTable(
                name: "AnswerCourse");

            migrationBuilder.DropTable(
                name: "AnswerLession");

            migrationBuilder.DropTable(
                name: "CourseImageVideo");

            migrationBuilder.DropTable(
                name: "LessionVideoImage");

            migrationBuilder.DropTable(
                name: "OtherFileUpload");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "QuestionFinal");

            migrationBuilder.DropTable(
                name: "QuestionLession");

            migrationBuilder.DropTable(
                name: "QuizCourse");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "HistorySubmitFinal");

            migrationBuilder.DropTable(
                name: "ReportUserScoreFinal");

            migrationBuilder.DropTable(
                name: "HistorySubmitLession");

            migrationBuilder.DropTable(
                name: "Lession");

            migrationBuilder.DropTable(
                name: "ReportUserScore");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
