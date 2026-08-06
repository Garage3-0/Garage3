using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Migrations
{
    /// <inheritdoc />
    public partial class IsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RegNbr",
                table: "ParkedVehicle",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_RegNbr",
                table: "ParkedVehicle",
                column: "RegNbr",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_RegNbr",
                table: "ParkedVehicle");

            migrationBuilder.AlterColumn<string>(
                name: "RegNbr",
                table: "ParkedVehicle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
