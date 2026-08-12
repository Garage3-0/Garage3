using System.ComponentModel.DataAnnotations;
using Garage3.Data;

namespace Garage3.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        //public required string RegistrationNumber { get; set; }

        private string _regNbr = string.Empty;

        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9]{3}$", ErrorMessage = "The Registration number needs to follow format ABC123.")]
        public required string RegNbr
        {
            get => _regNbr;
            set => _regNbr = value?.Trim().ToUpperInvariant() ?? string.Empty;
        }


        [StringLength(10, MinimumLength = 2)]
        public required string Color { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public required string Brand { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public required string Model { get; set; }

        [Range(2, 10)]
        public required int NumberOfWheels { get; set; }


        // TODO - change name to VehicleType when old VehicleType-model is removed
        public required int VehicleTypeNewId { get; set; }

        // Required reference navigation to principal
        public VehicleTypeNew VehicleTypeNew { get; set; } = null!;

        public ICollection<ParkingSession> ParkingSessions { get; } = new List<ParkingSession>();


        // TODO - add ApplicationUser 
        //public required string ApplicationUserId { get; set; }
        //public ApplicationUser? ApplicationUser { get; set; }  // TODO required !!!

    }
}
