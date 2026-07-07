using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ParkedVehicleViewModel
    {
        [Required(ErrorMessage = "Please select a vehicle type.")]
        [Display(Name = "Vehicle type")]
        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "Registration number is required.")]
        [StringLength(10, ErrorMessage = "Registration number is too long.")]
        [Display(Name = "Registration number")]
        public string RegNbr { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        [StringLength(10, ErrorMessage = "Color name is too long.")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [StringLength(20, ErrorMessage = "Brand name is too long.")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(10, ErrorMessage = "Model name is too long.")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Number of wheels is required.")]
        [Range(2, 10, ErrorMessage = "Number of wheels must be between 2 and 10.")]
        [Display(Name = "Number of wheels")]
        public int Wheels { get; set; }
    }
}
