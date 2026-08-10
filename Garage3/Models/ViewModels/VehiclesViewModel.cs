using Garage3.Models;

namespace Garage3.Models.ViewModels

{
    public class VehiclesViewModel
    {
        public required IEnumerable<ParkedVehicle> ParkedVehicles { get; set; }

    }
}
