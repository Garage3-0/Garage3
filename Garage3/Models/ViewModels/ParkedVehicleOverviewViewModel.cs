using Garage3.Models;

namespace Garage3.Models.ViewModels
{
    public class ParkedVehicleOverviewViewModel
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleTypeName { get; set; } = string.Empty;
        public string RegNbr { get; set; } = "";
        public string Color { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Wheels { get; set; }
        public DateTime Arrival { get; set; }
    }
}