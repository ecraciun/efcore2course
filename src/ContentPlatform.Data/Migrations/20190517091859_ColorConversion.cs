using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class ColorConversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleBackgroundColor",
                table: "BlogPosts",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleBackgroundColor",
                table: "BlogPosts");
        }
    }
}
