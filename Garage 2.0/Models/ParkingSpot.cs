using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Garage_3._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        public required int Number { get; set; }  // UK

        //=== What is Location? ===
        [StringLength(20)]
        public string Location { get; set; } = string.Empty;  // required???

        public bool IsOutOfService { get; set; } = false;
    }
}
