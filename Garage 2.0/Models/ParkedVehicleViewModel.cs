using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ParkedVehicleViewModel
    {
        [Required]
        public VehicleType VehicleType { get; set; }

        [Required]
        public string RegNbr { get; set; }

        [Required]
        [StringLength(10)]
        public string Color { get; set; }

        [Required]
        [StringLength(20)]
        public string Brand { get; set; }

        [Required]
        [StringLength(10)]
        public string Model { get; set; }

        [Range(2, 10)]
        public int Wheels { get; set; }
    }
}
