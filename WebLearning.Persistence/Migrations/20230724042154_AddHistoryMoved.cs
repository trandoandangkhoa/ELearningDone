using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class AddHistoryMoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AssetMoved",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AssetMovedHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMovedHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_Code",
                table: "AssetMoved",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMoved_AssetMovedHistory_Code",
                table: "AssetMoved",
                column: "Code",
                principalTable: "AssetMovedHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMoved_AssetMovedHistory_Code",
                table: "AssetMoved");

            migrationBuilder.DropTable(
                name: "AssetMovedHistory");

            migrationBuilder.DropIndex(
                name: "IX_AssetMoved_Code",
                table: "AssetMoved");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AssetMoved",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
