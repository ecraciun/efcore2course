using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class AddedShadowProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Publishers",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Publishers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Locations",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Contributions",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Contributions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Blogs",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BlogPosts",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "BlogPosts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Authors",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Authors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Authors");
        }
    }
}
