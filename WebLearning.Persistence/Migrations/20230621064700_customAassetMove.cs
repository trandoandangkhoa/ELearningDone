using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class customAassetMove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "AssetsMoved");

            migrationBuilder.AddColumn<Guid>(
                name: "AssetsDepartmentId",
                table: "AssetsMoved",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetsMoved_AssetsDepartmentId",
                table: "AssetsMoved",
                column: "AssetsDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetsMoved_AssetsDepartment_AssetsDepartmentId",
                table: "AssetsMoved",
                column: "AssetsDepartmentId",
                principalTable: "AssetsDepartment",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetsMoved_AssetsDepartment_AssetsDepartmentId",
                table: "AssetsMoved");

            migrationBuilder.DropIndex(
                name: "IX_AssetsMoved_AssetsDepartmentId",
                table: "AssetsMoved");

            migrationBuilder.DropColumn(
                name: "AssetsDepartmentId",
                table: "AssetsMoved");

            migrationBuilder.AddColumn<string>(
                name: "AssetId",
                table: "AssetsMoved",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
