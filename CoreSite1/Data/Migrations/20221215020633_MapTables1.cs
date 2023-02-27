using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSite1.Data.Migrations
{
    public partial class MapTables1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapStockID",
                table: "MapImage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapImage_MapStockID",
                table: "MapImage",
                column: "MapStockID");

            migrationBuilder.AddForeignKey(
                name: "FK_MapImage_MapStock_MapStockID",
                table: "MapImage",
                column: "MapStockID",
                principalTable: "MapStock",
                principalColumn: "MapStockID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapImage_MapStock_MapStockID",
                table: "MapImage");

            migrationBuilder.DropIndex(
                name: "IX_MapImage_MapStockID",
                table: "MapImage");

            migrationBuilder.DropColumn(
                name: "MapStockID",
                table: "MapImage");
        }
    }
}
