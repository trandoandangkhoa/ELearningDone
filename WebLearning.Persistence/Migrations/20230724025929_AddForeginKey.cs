using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddForeginKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMoved_Asset_AssestsId",
                table: "AssetMoved");

            migrationBuilder.DropIndex(
                name: "IX_AssetMoved_AssestsId",
                table: "AssetMoved");

            migrationBuilder.AlterColumn<string>(
                name: "AssestsId",
                table: "AssetMoved",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldAssestsId",
                table: "AssetMoved",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_OldAssestsId",
                table: "AssetMoved",
                column: "OldAssestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMoved_Asset_OldAssestsId",
                table: "AssetMoved",
                column: "OldAssestsId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMoved_Asset_OldAssestsId",
                table: "AssetMoved");

            migrationBuilder.DropIndex(
                name: "IX_AssetMoved_OldAssestsId",
                table: "AssetMoved");

            migrationBuilder.DropColumn(
                name: "OldAssestsId",
                table: "AssetMoved");

            migrationBuilder.AlterColumn<string>(
                name: "AssestsId",
                table: "AssetMoved",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_AssestsId",
                table: "AssetMoved",
                column: "AssestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMoved_Asset_AssestsId",
                table: "AssetMoved",
                column: "AssestsId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
