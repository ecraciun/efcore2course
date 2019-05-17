using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class NewSeedAfterOneToMany : Migration
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

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName", "PublisherId" },
                values: new object[] { 1, "a@a.a", "John", "Snow", 1 });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName", "PublisherId" },
                values: new object[] { 2, "b@b.b", "Arya", "Stark", 1 });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName", "PublisherId" },
                values: new object[] { 3, "c@c.c", "Margaery", "Tyrell", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 1);
        }
    }
}
