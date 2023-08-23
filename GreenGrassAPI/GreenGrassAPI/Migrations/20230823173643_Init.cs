using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenGrassAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CareInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemperatureRangeMin = table.Column<int>(type: "int", nullable: false),
                    TemperatureRangeMax = table.Column<int>(type: "int", nullable: false),
                    HumidityRangeMin = table.Column<int>(type: "int", nullable: false),
                    HumidityRangeMax = table.Column<int>(type: "int", nullable: false),
                    SoilType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prunning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lighting = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bursting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PottedSuggestions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WateringFrequency = table.Column<int>(type: "int", nullable: false),
                    FertilizingFrequency = table.Column<int>(type: "int", nullable: false),
                    RepottingFrequency = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    LastWateringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextWateringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastFertilizingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextFertilizingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PlantId",
                table: "Notifications",
                column: "PlantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_UserId",
                table: "Plants",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
