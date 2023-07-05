using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddTabelSupplierAndRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetsSupplierId",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssetsRepaired",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssestsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationRepaired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRepaired = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsRepaired", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsRepaired_Assets_AssestsId",
                        column: x => x.AssestsId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssetsSupplier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyTaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsSupplier", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetsSupplierId",
                table: "Assets",
                column: "AssetsSupplierId",
                unique: true,
                filter: "[AssetsSupplierId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssetsRepaired_AssestsId",
                table: "AssetsRepaired",
                column: "AssestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AssetsSupplier_AssetsSupplierId",
                table: "Assets",
                column: "AssetsSupplierId",
                principalTable: "AssetsSupplier",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AssetsSupplier_AssetsSupplierId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "AssetsRepaired");

            migrationBuilder.DropTable(
                name: "AssetsSupplier");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetsSupplierId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AssetsSupplierId",
                table: "Assets");
        }
    }
}
