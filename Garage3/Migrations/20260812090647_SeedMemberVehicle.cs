using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage3.Migrations
{
    /// <inheritdoc />
    public partial class SeedMemberVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles");

            migrationBuilder.DeleteData(
                table: "ParkingSession",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkingSession",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParkingSession",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "ParkingSession",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRateAtCheckin",
                table: "ParkingSession",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "ParkingSession",
                type: "decimal(10, 2",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRateAtCheckin",
                table: "ParkingSession",
                type: "decimal(10, 2",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "ApplicationUserId", "Brand", "Color", "Model", "NumberOfWheels", "RegistrationNumber", "VehicleTypeNewId" },
                values: new object[,]
                {
                    { 1, null, "Alfa Romeo", "Blue", "X99", 4, "AAA111", 2 },
                    { 2, null, "Ford", "Blue", "Fiesta", 4, "BBB222", 2 },
                    { 3, null, "Honda", "Red", "CBX750", 2, "CCC333", 3 }
                });

            migrationBuilder.InsertData(
                table: "ParkingSession",
                columns: new[] { "Id", "CheckInTime", "CheckOutTime", "HourlyRateAtCheckin", "ParkingSpotId", "TotalPrice", "VehicleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9.9m, 1, null, 1 },
                    { 2, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9.9m, 2, null, 2 },
                    { 3, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9.9m, 3, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles",
                column: "RegistrationNumber",
                unique: true);
        }
    }
}
