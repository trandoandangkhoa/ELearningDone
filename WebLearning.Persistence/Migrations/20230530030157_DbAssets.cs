using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class DbAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetsCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetsDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetsStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetsSubCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsSubCategory_AssetsCategory_AssetsCategoryId",
                        column: x => x.AssetsCategoryId,
                        principalTable: "AssetsCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetsSubCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AssetsDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsStatusId = table.Column<int>(type: "int", nullable: false),
                    DateUsed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateChecked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Spec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetsCategory_AssetsCategoryId",
                        column: x => x.AssetsCategoryId,
                        principalTable: "AssetsCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_AssetsDepartment_AssetsDepartmentId",
                        column: x => x.AssetsDepartmentId,
                        principalTable: "AssetsDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_AssetsStatus_AssetsStatusId",
                        column: x => x.AssetsStatusId,
                        principalTable: "AssetsStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetsCategoryId",
                table: "Assets",
                column: "AssetsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetsDepartmentId",
                table: "Assets",
                column: "AssetsDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetsStatusId",
                table: "Assets",
                column: "AssetsStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetsSubCategory_AssetsCategoryId",
                table: "AssetsSubCategory",
                column: "AssetsCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "AssetsSubCategory");

            migrationBuilder.DropTable(
                name: "AssetsDepartment");

            migrationBuilder.DropTable(
                name: "AssetsStatus");

            migrationBuilder.DropTable(
                name: "AssetsCategory");
        }
    }
}
