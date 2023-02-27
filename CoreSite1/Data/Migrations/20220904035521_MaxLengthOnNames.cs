using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSite1.Data.Migrations
{
    public partial class MaxLengthOnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OldUnitInStock",
                table: "Variants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Products",
                nullable: false,
                defaultValue: 0m);

           

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemOrderType",
                table: "Carts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldUnitInStock",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Products");

           

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ItemOrderType",
                table: "Carts");
        }
    }
}
