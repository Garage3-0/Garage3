using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Garage_3._0.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }

        public required VehicleTypes VehicleTypes { get; set; }

        public required string RegNbr {  get; set; }

        [StringLength(10)]
        public required string Color { get; set; }

        [StringLength(20)]
        public required string Brand { get; set; }

        [StringLength(10)]
        public required string Model { get; set; }

        [Range(2, 10)]
        public int Wheels { get; set; }

        public DateTime Arrival { get; set; } = DateTime.Now;

    }
}