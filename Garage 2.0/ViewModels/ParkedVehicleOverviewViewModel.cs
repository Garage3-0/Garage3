using Garage_2._0.Models;

namespace Garage_2._0.ViewModels
{
    public class ParkedVehicleOverviewViewModel
    {
        public int Id { get; set; }

        public VehicleType VehicleType { get; set; }

        public string RegNbr { get; set; } = "";

        public DateTime Arrival { get; set; }
    }
}