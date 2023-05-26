using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddOptionAndCorrectAnswerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QuestionLession");

            migrationBuilder.DropColumn(
                name: "OptA",
                table: "QuestionLession");

            migrationBuilder.DropColumn(
                name: "OptB",
                table: "QuestionLession");

            migrationBuilder.DropColumn(
                name: "OptC",
                table: "QuestionLession");

            migrationBuilder.DropColumn(
                name: "OptD",
                table: "QuestionLession");

            migrationBuilder.CreateTable(
                name: "CorrectAnswerLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswerLession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswerLession_QuestionLession_QuestionLessionId",
                        column: x => x.QuestionLessionId,
                        principalTable: "QuestionLession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionLessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionLessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionLessions_QuestionLession_QuestionLessionId",
                        column: x => x.QuestionLessionId,
                        principalTable: "QuestionLession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswerLession_QuestionLessionId",
                table: "CorrectAnswerLession",
                column: "QuestionLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionLessions_QuestionLessionId",
                table: "OptionLessions",
                column: "QuestionLessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectAnswerLession");

            migrationBuilder.DropTable(
                name: "OptionLessions");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptA",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptB",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptC",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptD",
                table: "QuestionLession",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
