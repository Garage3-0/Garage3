using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage3.Migrations
{
    /// <inheritdoc />
    public partial class MovedSeedVehicleTypsNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleTypeNew_Name",
                table: "VehicleTypeNew");

            migrationBuilder.DeleteData(
                table: "VehicleTypeNew",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VehicleTypeNew",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VehicleTypeNew",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VehicleTypeNew",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VehicleTypeNew",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "VehicleTypeNew",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bus" },
                    { 2, "Car" },
                    { 3, "Motorcycle" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeNew_Name",
                table: "VehicleTypeNew",
                column: "Name",
                unique: true);
        }
    }
}
