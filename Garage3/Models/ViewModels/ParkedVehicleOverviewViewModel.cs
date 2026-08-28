using Garage3.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class ParkedVehicleOverviewViewModel
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }

        [Display(Name = "Vehicle Type")]
        public string VehicleTypeName { get; set; } = string.Empty;

        //[Display(Name = "Registration Number")]
        [Display(Name = "Reg.number")]
        public string RegNbr { get; set; } = "";
        public string Color { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Wheels { get; set; }
        public DateTime Arrival { get; set; }
    }
}