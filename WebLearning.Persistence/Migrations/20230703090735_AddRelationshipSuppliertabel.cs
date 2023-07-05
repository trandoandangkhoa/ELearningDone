using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddRelationshipSuppliertabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetsSupplierId",
                table: "Assets");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetsSupplierId",
                table: "Assets",
                column: "AssetsSupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetsSupplierId",
                table: "Assets");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetsSupplierId",
                table: "Assets",
                column: "AssetsSupplierId",
                unique: true,
                filter: "[AssetsSupplierId] IS NOT NULL");
        }
    }
}
