using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        [Required(ErrorMessage = "Please select a vehicle type.")]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }

        public SelectList? VehicleTypes { get; set; }

        [Required(ErrorMessage = "Registration number is required.")]
        [Display(Name = "Registration Number")]
        public string RegNbr { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Color { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Model { get; set; } = string.Empty;

        [Range(2, 10, ErrorMessage = "Wheels must be between 2 and 10.")]
        public int Wheels { get; set; }
    }
}