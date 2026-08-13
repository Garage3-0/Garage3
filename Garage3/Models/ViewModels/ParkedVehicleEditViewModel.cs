using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class ParkedVehicleEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You need to select a vehicle type")]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        public SelectList? VehicleTypes { get; set; }

        [Required(ErrorMessage = "Registration number is required")]
        public string RegNbr { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color is required")]
        [StringLength(10)]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(20)]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(10)]
        public string Model { get; set; } = string.Empty;

        [Range(2, 10, ErrorMessage = "Number of wheels must be between 2 and 10.")]
        public int Wheels { get; set; }

        public DateTime Arrival { get; set; }
    }
}