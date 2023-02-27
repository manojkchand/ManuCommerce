using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSite1.Data.Migrations
{
    public partial class HeaderImgFieldBlogpage_PageTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderImage",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderImage",
                table: "Pages");
        }
    }
}
