using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public required string RegistrationNumber { get; set; }

        [StringLength(10, MinimumLength = 2)]
        public required string Color { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public required string Brand { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public required string Model { get; set; }

        [Range(2, 10)]
        public required int NumberOfWheels { get; set; }


        public required int VehicleTypeId { get; set; }  // Required foreign key property
        public VehicleType VehicleType { get; set; } = null!;



        //=== Add ApplicationUser ===
        //public int OwnerId { get; set; }
        //public ApplicationUser int { get; set; }  // "string" according to EF-chart !?
    }
}
