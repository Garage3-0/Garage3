using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_3._0.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    RegNbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "Arrival", "Brand", "Color", "Model", "RegNbr", "VehicleType", "Wheels" },
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicle");
        }
    }
}
