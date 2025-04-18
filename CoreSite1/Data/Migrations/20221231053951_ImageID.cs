﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSite1.Data.Migrations
{
    public partial class ImageID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "MapImage",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "MapImage");
        }
    }
}
