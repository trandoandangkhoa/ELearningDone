using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLearning.Persistence.Migrations
{
    public partial class RemoveSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Assets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
