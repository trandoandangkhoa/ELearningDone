using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddDependenceOfLessionAndOtherFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherFileUpload_Course_CourseId",
                table: "OtherFileUpload");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "OtherFileUpload",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "LessionId",
                table: "OtherFileUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OtherFileUpload_LessionId",
                table: "OtherFileUpload",
                column: "LessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherFileUpload_Course_CourseId",
                table: "OtherFileUpload",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherFileUpload_Lession_LessionId",
                table: "OtherFileUpload",
                column: "LessionId",
                principalTable: "Lession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherFileUpload_Course_CourseId",
                table: "OtherFileUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherFileUpload_Lession_LessionId",
                table: "OtherFileUpload");

            migrationBuilder.DropIndex(
                name: "IX_OtherFileUpload_LessionId",
                table: "OtherFileUpload");

            migrationBuilder.DropColumn(
                name: "LessionId",
                table: "OtherFileUpload");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "OtherFileUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherFileUpload_Course_CourseId",
                table: "OtherFileUpload",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
