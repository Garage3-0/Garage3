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
                    { 9, new DateTime(2026, 6, 28, 15, 45, 0, 0, DateTimeKind.Unspecified), "Volvo", "White", "AZ34", "JYT628", 2, 8 },
                    { 10, new DateTime(2026, 7, 8, 10, 18, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "V30", "DER421", 1, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
