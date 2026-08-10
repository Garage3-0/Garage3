using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_3._0.Migrations
{
    /// <inheritdoc />
    public partial class addedParkedVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkedVehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTypes = table.Column<int>(type: "int", nullable: false),
                    RegNbr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Wheels = table.Column<int>(type: "int", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsOutOfService = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumberOfWheels = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleType_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "Arrival", "Brand", "Color", "Model", "RegNbr", "VehicleTypes", "Wheels" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 6, 10, 59, 0, 0, DateTimeKind.Unspecified), "Volvo", "Red", "V60", "ABC123", 0, 4 },
                    { 2, new DateTime(2026, 7, 4, 11, 59, 0, 0, DateTimeKind.Unspecified), "Toyota", "Blue", "A50", "BGD567", 1, 2 },
                    { 3, new DateTime(2026, 6, 5, 9, 10, 0, 0, DateTimeKind.Unspecified), "Saab", "Yellow", "H88", "KLI908", 2, 10 },
                    { 4, new DateTime(2026, 7, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), "Toyota", "Black", "X76", "TRE654", 1, 2 },
                    { 5, new DateTime(2026, 7, 1, 10, 45, 0, 0, DateTimeKind.Unspecified), "Saab", "Blue", "C50", "DUN584", 0, 4 },
                    { 6, new DateTime(2026, 6, 28, 14, 50, 0, 0, DateTimeKind.Unspecified), "Volvo", "White", "BG70", "PLG327", 2, 10 },
                    { 7, new DateTime(2026, 6, 30, 16, 25, 0, 0, DateTimeKind.Unspecified), "Mazda", "Black", "BT50", "NJG968", 0, 4 },
                    { 8, new DateTime(2026, 7, 6, 11, 18, 0, 0, DateTimeKind.Unspecified), "Toyota", "White", "A50", "RFM596", 0, 4 },
                    { 9, new DateTime(2026, 6, 28, 15, 45, 0, 0, DateTimeKind.Unspecified), "Volvo", "White", "AZ34", "JYT628", 2, 8 },
                    { 10, new DateTime(2026, 7, 8, 10, 18, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "V30", "DER421", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsOutOfService", "Location", "Number" },
                values: new object[,]
                {
                    { 1, false, "", 100 },
                    { 2, false, "", 101 },
                    { 3, false, "", 102 }
                });

            migrationBuilder.InsertData(
                table: "VehicleType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bus" },
                    { 2, "Car" },
                    { 3, "Motorcycle" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "Color", "Model", "NumberOfWheels", "RegistrationNumber", "VehicleTypeId" },
                values: new object[,]
                {
                    { 1, "Alfa Romeo", "Blue", "X99", 4, "AAA111", 2 },
                    { 2, "Ford", "Blue", "Fiesta", 4, "BBB222", 2 },
                    { 3, "Honda", "Red", "CBX750", 2, "CCC333", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_RegNbr",
                table: "ParkedVehicle",
                column: "RegNbr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_Number",
                table: "ParkingSpots",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleType_Name",
                table: "VehicleType",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicle");

            migrationBuilder.DropTable(
                name: "ParkingSpots");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "VehicleType");
        }
    }
}
