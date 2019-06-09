using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackMachine.Domain.Migrations
{
    public partial class OwnedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoneyInside_FiveDollarCount",
                table: "SnackMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoneyInside_OneCentCount",
                table: "SnackMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoneyInside_OneDollarCount",
                table: "SnackMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoneyInside_QuarterCount",
                table: "SnackMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoneyInside_TenCentCount",
                table: "SnackMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoneyInside_TwentyDollarCount",
                table: "SnackMachines",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoneyInside_FiveDollarCount",
                table: "SnackMachines");

            migrationBuilder.DropColumn(
                name: "MoneyInside_OneCentCount",
                table: "SnackMachines");

            migrationBuilder.DropColumn(
                name: "MoneyInside_OneDollarCount",
                table: "SnackMachines");

            migrationBuilder.DropColumn(
                name: "MoneyInside_QuarterCount",
                table: "SnackMachines");

            migrationBuilder.DropColumn(
                name: "MoneyInside_TenCentCount",
                table: "SnackMachines");

            migrationBuilder.DropColumn(
                name: "MoneyInside_TwentyDollarCount",
                table: "SnackMachines");
        }
    }
}
