using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenGrassAPI.Migrations
{
    public partial class UpdateNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FertilizingPeriod",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WateringPeriod",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FertilizingPeriod",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "WateringPeriod",
                table: "Notifications");
        }
    }
}
