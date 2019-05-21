using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class AddedOwnedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "BlogPosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Title",
                table: "BlogPosts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "Metadata_Title",
                table: "BlogPosts");
        }
    }
}
