using Garage3.Data;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models
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

        public ICollection<ParkingSession> ParkingSessions { get; } = new List<ParkingSession>();


        // TODO - change name to VehicleType when old VehicleType-model is removed
        public required int VehicleTypeNewId { get; set; }
        public required string ApplicationUserId { get; set; }


        // Required reference navigation to principal
        public VehicleTypeNew VehicleTypeNew { get; set; } = null!;
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
