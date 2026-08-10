using System.ComponentModel.DataAnnotations;

namespace Garage3.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        public required int Number { get; set; }  // UK

        [StringLength(20)]
        public string Location { get; set; } = string.Empty;  // Not in use

        public bool IsOutOfService { get; set; } = false;
    }
}
