using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class AddAuthorDataInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName" },
                values: new object[] { 1, "a@a.a", "John", "Snow" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName" },
                values: new object[] { 2, "b@b.b", "Arya", "Stark" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName" },
                values: new object[] { 3, "c@c.c", "Margaery", "Tyrell" });
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
        }
    }
}
