using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class RemoveAlertTableAndAddNotificationTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.RenameColumn(
                name: "New",
                table: "QuizMonthly",
                newName: "Notify");

            migrationBuilder.RenameColumn(
                name: "New",
                table: "QuizLession",
                newName: "Notify");

            migrationBuilder.RenameColumn(
                name: "New",
                table: "QuizCourse",
                newName: "Notify");

            migrationBuilder.RenameColumn(
                name: "New",
                table: "LessionVideoImage",
                newName: "Notify");

            migrationBuilder.RenameColumn(
                name: "New",
                table: "Lession",
                newName: "Notify");

            migrationBuilder.RenameColumn(
                name: "New",
                table: "Course",
                newName: "Notify");

            migrationBuilder.AddColumn<string>(
                name: "DescNotify",
                table: "QuizMonthly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescNotify",
                table: "QuizLession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescNotify",
                table: "QuizCourse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescNotify",
                table: "LessionVideoImage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescNotify",
                table: "Lession",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescNotify",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescNotify",
                table: "QuizMonthly");

            migrationBuilder.DropColumn(
                name: "DescNotify",
                table: "QuizLession");

            migrationBuilder.DropColumn(
                name: "DescNotify",
                table: "QuizCourse");

            migrationBuilder.DropColumn(
                name: "DescNotify",
                table: "LessionVideoImage");

            migrationBuilder.DropColumn(
                name: "DescNotify",
                table: "Lession");

            migrationBuilder.DropColumn(
                name: "DescNotify",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Notify",
                table: "QuizMonthly",
                newName: "New");

            migrationBuilder.RenameColumn(
                name: "Notify",
                table: "QuizLession",
                newName: "New");

            migrationBuilder.RenameColumn(
                name: "Notify",
                table: "QuizCourse",
                newName: "New");

            migrationBuilder.RenameColumn(
                name: "Notify",
                table: "LessionVideoImage",
                newName: "New");

            migrationBuilder.RenameColumn(
                name: "Notify",
                table: "Lession",
                newName: "New");

            migrationBuilder.RenameColumn(
                name: "Notify",
                table: "Course",
                newName: "New");

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionVideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alert_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CourseId",
                table: "Alert",
                column: "CourseId");
        }
    }
}
