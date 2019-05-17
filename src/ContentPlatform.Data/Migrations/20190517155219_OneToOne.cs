using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentPlatform.Data.Migrations
{
    public partial class OneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PublisherId",
                table: "Locations",
                column: "PublisherId",
                unique: true,
                filter: "[PublisherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Publishers_PublisherId",
                table: "Locations",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Publishers_PublisherId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PublisherId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Locations");
        }
    }
}
