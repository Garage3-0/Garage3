using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Vehicles",
                newName: "RegNbr");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles",
                newName: "IX_Vehicles_RegNbr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegNbr",
                table: "Vehicles",
                newName: "RegistrationNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_RegNbr",
                table: "Vehicles",
                newName: "IX_Vehicles_RegistrationNumber");
        }
    }
}
