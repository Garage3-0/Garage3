using System.ComponentModel.DataAnnotations.Schema;

namespace Garage3.Models
{
    public class ParkingSession
    {
        public int Id { get; set; }

        public DateTime CheckInTime { get; set; } = DateTime.Now;

        public DateTime? CheckOutTime { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal HourlyRateAtCheckin { get; set; }  // Is set at creation

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalPrice { get; set; }


        // Required foreign key property
        public int VehicleId { get; set; }
        public int ParkingSpotId { get; set; }


        // Required reference navigation to principal
        public Vehicle Vehicle { get; set; } = null!;
        public ParkingSpot ParkingSpot { get; set; } = null!;
    }
}
