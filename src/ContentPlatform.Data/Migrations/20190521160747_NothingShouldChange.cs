using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class NothingShouldChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                    CREATE VIEW View_BlogPostCount AS
                    SELECT b.Title, b.Url, COUNT(p.PostId) as Count
                    FROM Blogs b
                    JOIN BlogPosts p on p.BlogId = b.BlogId
                    GROUP BY b.Title, b.Url
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_BlogPostCount");
        }
    }
}