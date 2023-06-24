using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddAssetsCategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetsSubCategory_AssetsCategory_AssetsCategoryId",
                table: "AssetsSubCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssetsCategoryId",
                table: "AssetsSubCategory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetsSubCategory_AssetsCategory_AssetsCategoryId",
                table: "AssetsSubCategory",
                column: "AssetsCategoryId",
                principalTable: "AssetsCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetsSubCategory_AssetsCategory_AssetsCategoryId",
                table: "AssetsSubCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssetsCategoryId",
                table: "AssetsSubCategory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetsSubCategory_AssetsCategory_AssetsCategoryId",
                table: "AssetsSubCategory",
                column: "AssetsCategoryId",
                principalTable: "AssetsCategory",
                principalColumn: "Id");
        }
    }
}
