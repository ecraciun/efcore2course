using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class SeededPublishers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherId", "MainWebsite", "Name" },
                values: new object[] { 1, "http://a.a.a", "G.R.R.M." });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherId", "MainWebsite", "Name" },
                values: new object[] { 2, "http://contoso.com", "Contoso" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 2);
        }
    }
}
