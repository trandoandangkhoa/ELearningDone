using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class customAassetMove1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetsMoved_AssetsDepartment_AssetsDepartmentId",
                table: "AssetsMoved");

            migrationBuilder.DropColumn(
                name: "DepartmentUsed",
                table: "AssetsMoved");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssetsDepartmentId",
                table: "AssetsMoved",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetsMoved_AssetsDepartment_AssetsDepartmentId",
                table: "AssetsMoved",
                column: "AssetsDepartmentId",
                principalTable: "AssetsDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetsMoved_AssetsDepartment_AssetsDepartmentId",
                table: "AssetsMoved");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssetsDepartmentId",
                table: "AssetsMoved",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUsed",
                table: "AssetsMoved",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetsMoved_AssetsDepartment_AssetsDepartmentId",
                table: "AssetsMoved",
                column: "AssetsDepartmentId",
                principalTable: "AssetsDepartment",
                principalColumn: "Id");
        }
    }
}
