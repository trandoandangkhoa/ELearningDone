using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class RemoveIsDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "OtherFileUpload");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "LessionVideoImage");

            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "Lession");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "CourseImageVideo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "OtherFileUpload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "LessionVideoImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompletedTime",
                table: "Lession",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "CourseImageVideo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
