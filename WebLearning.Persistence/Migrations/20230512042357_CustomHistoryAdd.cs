using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class CustomHistoryAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OldCodeId",
                table: "HistoryAddSlots",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TypedSubmit",
                table: "HistoryAddSlots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAddSlots_RoomId",
                table: "HistoryAddSlots",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAddSlots_Rooms_RoomId",
                table: "HistoryAddSlots",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAddSlots_Rooms_RoomId",
                table: "HistoryAddSlots");

            migrationBuilder.DropIndex(
                name: "IX_HistoryAddSlots_RoomId",
                table: "HistoryAddSlots");

            migrationBuilder.DropColumn(
                name: "OldCodeId",
                table: "HistoryAddSlots");

            migrationBuilder.DropColumn(
                name: "TypedSubmit",
                table: "HistoryAddSlots");
        }
    }
}
