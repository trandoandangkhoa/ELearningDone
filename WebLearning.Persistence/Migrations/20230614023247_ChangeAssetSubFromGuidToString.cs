using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class ChangeAssetSubFromGuidToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetsSubCategory");

            migrationBuilder.DropColumn(
                name: "AssetsSubCategoryId",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "AssetSubCategory",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetSubCategory",
                table: "Assets");

            migrationBuilder.AddColumn<Guid>(
                name: "AssetsSubCategoryId",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AssetsSubCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetsCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsSubCategory_AssetsCategory_AssetsCategoryId",
                        column: x => x.AssetsCategoryId,
                        principalTable: "AssetsCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetsSubCategory_AssetsCategoryId",
                table: "AssetsSubCategory",
                column: "AssetsCategoryId");
        }
    }
}
