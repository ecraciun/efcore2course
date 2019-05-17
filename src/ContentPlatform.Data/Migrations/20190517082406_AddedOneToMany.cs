using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class AddedOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Blogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Authors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_PublisherId",
                table: "Blogs",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_PublisherId",
                table: "Authors",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Publishers_PublisherId",
                table: "Authors",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Publishers_PublisherId",
                table: "Blogs",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Blogs_BlogId",
                table: "Posts",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Publishers_PublisherId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Publishers_PublisherId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Blogs_BlogId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_BlogId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_PublisherId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Authors_PublisherId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Authors");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "a@a.a", "John", "Snow" },
                    { 2, "b@b.b", "Arya", "Stark" },
                    { 3, "c@c.c", "Margaery", "Tyrell" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherId", "MainWebsite", "Name" },
                values: new object[,]
                {
                    { 1, "http://a.a.a", "G.R.R.M." },
                    { 2, "http://contoso.com", "Contoso" }
                });
        }
    }
}
