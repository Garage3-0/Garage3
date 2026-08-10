using Garage3.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public required int Id { get; set; }

        [Display(Name = "Vehicle type")]
        public VehicleType? VehicleType { get; set; }

        [Display(Name = "Reg.nbr")]
        public required string RegNbr { get; set; }

        public string? Color { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int? Wheels { get; set; }

        public required DateTime Arrival { get; set; }

        [Display(Name = "Checkout time")]
        public required DateTime CheckoutTime { get; set; }

        public required int ParkedDays { get; set; }

        public required int ParkedHours { get; set; }

        public required int ParkedMinutes { get; set; }

        public required int Price { get; set; }

        public required int PricePerHour { get; set; }
    }
}
