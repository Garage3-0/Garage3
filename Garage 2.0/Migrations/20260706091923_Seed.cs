using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "Arrival", "Brand", "Color", "Model", "RegNbr", "VehicleType", "Wheels" },
                values: new object[,]
                {
                    { 3, new DateTime(2026, 6, 5, 9, 10, 0, 0, DateTimeKind.Unspecified), "Saab", "Yellow", "H88", "KLI908", 2, 10 },
                    { 4, new DateTime(2026, 7, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), "Toyota", "Black", "X76", "TRE654", 1, 2 },
                    { 5, new DateTime(2026, 7, 1, 10, 45, 0, 0, DateTimeKind.Unspecified), "Saab", "Blue", "C50", "DUN584", 0, 4 },
                    { 6, new DateTime(2026, 6, 28, 14, 50, 0, 0, DateTimeKind.Unspecified), "Volvo", "White", "BG70", "PLG327", 2, 10 },
                    { 7, new DateTime(2026, 6, 30, 16, 25, 0, 0, DateTimeKind.Unspecified), "Mazda", "Black", "BT50", "NJG968", 0, 4 },
                    { 8, new DateTime(2026, 7, 6, 11, 18, 0, 0, DateTimeKind.Unspecified), "Toyota", "White", "A50", "RFM596", 0, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
