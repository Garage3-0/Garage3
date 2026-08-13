using Garage3.Models;

namespace Garage3.Models.ViewModels

{
    public class VehiclesViewModel
    {
        public required IEnumerable<Vehicle> ParkedVehicles { get; set; }

    }
}
