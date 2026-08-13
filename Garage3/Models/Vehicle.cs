using Garage3.Data;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleTypeNew? VehicleType { get; set; }

        //public required string RegistrationNumber { get; set; }

        public required string RegNbr { get; set; }

        //[Required]
        //[RegularExpression(@"^[A-Z]{3}[0-9]{3}$", ErrorMessage = "The Registration number needs to follow format ABC123.")]
        //public required string RegistrationNumber
        //{
         //   get => _regNbr;
         //   set => _regNbr = value?.Trim().ToUpperInvariant() ?? string.Empty;
        //}


        [StringLength(10, MinimumLength = 2)]
        public required string Color { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public required string Brand { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public required string Model { get; set; }

        [Range(2, 10)]
        public required int NumberOfWheels { get; set; }

        public DateTime Arrival { get; set; } = DateTime.Now;
        public ICollection<ParkingSession> ParkingSessions { get; } = new List<ParkingSession>();


        // TODO - change name to VehicleType when old VehicleType-model is removed
        public required int VehicleTypeNewId { get; set; }
        public required string ApplicationUserId { get; set; } = string.Empty;


        // Required reference navigation to principal
        public VehicleTypeNew VehicleTypeNew { get; set; } = null!;
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
