using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class removeDescInLession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lession",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
