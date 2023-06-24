using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class AddAssetsMoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetsMovedStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsMovedStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetsMoved",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovedStatus = table.Column<int>(type: "int", nullable: false),
                    NumBravo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateMoved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssestsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsMoved", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsMoved_Assets_AssestsId",
                        column: x => x.AssestsId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssetsMoved_AssetsMovedStatus_MovedStatus",
                        column: x => x.MovedStatus,
                        principalTable: "AssetsMovedStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetsMoved_AssestsId",
                table: "AssetsMoved",
                column: "AssestsId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetsMoved_MovedStatus",
                table: "AssetsMoved",
                column: "MovedStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetsMoved");

            migrationBuilder.DropTable(
                name: "AssetsMovedStatus");
        }
    }
}
