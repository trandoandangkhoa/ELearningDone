using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class UpdateAssetMoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMoved_AssetDepartment_AssetsDepartmentId",
                table: "AssetMoved");

            migrationBuilder.DropIndex(
                name: "IX_AssetMoved_AssetsDepartmentId",
                table: "AssetMoved");

            migrationBuilder.RenameColumn(
                name: "AssetsDepartmentId",
                table: "AssetMoved",
                newName: "OldDepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "AssetNote",
                table: "AssetMoved",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetStatus",
                table: "AssetMoved",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewDepartmentId",
                table: "AssetMoved",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "AssetMoved",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetNote",
                table: "AssetMoved");

            migrationBuilder.DropColumn(
                name: "AssetStatus",
                table: "AssetMoved");

            migrationBuilder.DropColumn(
                name: "NewDepartmentId",
                table: "AssetMoved");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "AssetMoved");

            migrationBuilder.RenameColumn(
                name: "OldDepartmentId",
                table: "AssetMoved",
                newName: "AssetsDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_AssetsDepartmentId",
                table: "AssetMoved",
                column: "AssetsDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMoved_AssetDepartment_AssetsDepartmentId",
                table: "AssetMoved",
                column: "AssetsDepartmentId",
                principalTable: "AssetDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
