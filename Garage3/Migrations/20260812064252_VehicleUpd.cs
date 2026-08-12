using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage3.Migrations
{
    /// <inheritdoc />
    public partial class VehicleUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParkingSpots_Number",
                table: "ParkingSpots");

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsOutOfService", "Location", "Number" },
                values: new object[,]
                {
                    { 1, false, "", 100 },
                    { 2, false, "", 101 },
                    { 3, false, "", 102 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_Number",
                table: "ParkingSpots",
                column: "Number",
                unique: true);
        }
    }
}
