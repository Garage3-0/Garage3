using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class NewParkVehicle
    {
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Display(Name = "Parking spot")]
        public int ParkingSpotId { get; set; }


        // Data for select list, etc
        public SelectList? Vehicles { get; set; }
        public SelectList? ParkingSpots { get; set; }
    }
}
