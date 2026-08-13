//using Garage3.Data;
//using System.ComponentModel.DataAnnotations;

//namespace Garage3.Models
//{
//    public class ParkedVehicle
//    {
//        public int Id { get; set; }

//        public int VehicleTypeId { get; set; }
//        public VehicleTypeNew? VehicleType { get; set; }
//        //public required string RegNbr { get; set; }

//        [StringLength(10)]
//        public required string Color { get; set; }

//        [StringLength(20)]
//        public required string Brand { get; set; }

//        [StringLength(10)]
//        public required string Model { get; set; }

//        [Range(2, 10)]
//        public int Wheels { get; set; }

//        public DateTime Arrival { get; set; } = DateTime.Now;

//        public string ApplicationUserId { get; set; } = string.Empty;

//        public ApplicationUser? ApplicationUser { get; set; }
//    }
//}