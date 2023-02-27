using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSite1.Data.Migrations
{
    public partial class PageIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PCategorys",
                columns: table => new
                {
                    PageCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ParentCategoryId = table.Column<int>(nullable: false),
                    PageTempleteId = table.Column<int>(nullable: false),
                    Checked = table.Column<bool>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    ParentPageCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCategorys", x => x.PageCategoryId);
                    table.ForeignKey(
                        name: "FK_PCategorys_PCategorys_ParentPageCategoryId",
                        column: x => x.ParentPageCategoryId,
                        principalTable: "PCategorys",
                        principalColumn: "PageCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PTemplate",
                columns: table => new
                {
                    PageTemplateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TempleteURL = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true),
                    SideBar = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PTemplate", x => x.PageTemplateId);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    PageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    PageTempleteId = table.Column<int>(nullable: false),
                    PageName = table.Column<string>(maxLength: 350, nullable: true),
                    Heading = table.Column<string>(maxLength: 350, nullable: true),
                    SubHeading = table.Column<string>(maxLength: 350, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    PageCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_Pages_PCategorys_PageCategoryId",
                        column: x => x.PageCategoryId,
                        principalTable: "PCategorys",
                        principalColumn: "PageCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pages_PageCategoryId",
                table: "Pages",
                column: "PageCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PCategorys_ParentPageCategoryId",
                table: "PCategorys",
                column: "ParentPageCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "PTemplate");

            migrationBuilder.DropTable(
                name: "PCategorys");
        }
    }
}
