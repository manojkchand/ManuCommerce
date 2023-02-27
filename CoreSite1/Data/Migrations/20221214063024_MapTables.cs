using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSite1.Data.Migrations
{
    public partial class MapTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapStockID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapImage",
                columns: table => new
                {
                    MapImageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    LocationName = table.Column<string>(nullable: true),
                    MapShape = table.Column<int>(nullable: false),
                    coords = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapImage", x => x.MapImageID);
                });

            migrationBuilder.CreateTable(
                name: "MapStock",
                columns: table => new
                {
                    MapStockID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    MapImageID = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapStock", x => x.MapStockID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MapStockID",
                table: "Products",
                column: "MapStockID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MapStock_MapStockID",
                table: "Products",
                column: "MapStockID",
                principalTable: "MapStock",
                principalColumn: "MapStockID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MapStock_MapStockID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "MapImage");

            migrationBuilder.DropTable(
                name: "MapStock");

            migrationBuilder.DropIndex(
                name: "IX_Products_MapStockID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MapStockID",
                table: "Products");
        }
    }
}
