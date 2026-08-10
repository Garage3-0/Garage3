namespace Garage_3._0.Models.ViewModels
{
    public class ParkedVehicleOverviewViewModel
    {
        public int Id { get; set; }

        public VehicleType VehicleType { get; set; }

        public string RegNbr { get; set; } = "";

        public string Color { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Wheels { get; set; }

        public DateTime Arrival { get; set; }
    }
}