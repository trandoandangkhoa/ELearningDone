using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class OneOneRelationShipAvatarToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Avatar_AccountId",
                table: "Avatar");

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_AccountId",
                table: "Avatar",
                column: "AccountId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Avatar_AccountId",
                table: "Avatar");

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_AccountId",
                table: "Avatar",
                column: "AccountId");
        }
    }
}
