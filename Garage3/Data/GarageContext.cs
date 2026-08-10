using Garage_3._0.Models;
using Garage3.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

public class GarageContext(DbContextOptions<GarageContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ParkedVehicle>().HasIndex(v => v.RegNbr).IsUnique();
        modelBuilder.Entity<ParkedVehicle>().HasData(
            new ParkedVehicle()
            {
                Id = 1,
                VehicleType = VehicleType.Car,
                RegNbr = "ABC123",
                Color = "Red",
                Brand = "Volvo",
                Model = "V60",
                Wheels = 4,
                Arrival = new DateTime(2026,7,6, 10,59,00),
            },

            new ParkedVehicle()
            {
                Id = 2,
                VehicleType = VehicleType.Motorcycle,
                RegNbr = "BGD567",
                Color = "Blue",
                Brand = "Toyota",
                Model = "A50",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 4, 11, 59, 00),
            },

            new ParkedVehicle()
            {
                Id = 3,
                VehicleType = VehicleType.Bus,
                RegNbr = "KLI908",
                Color = "Yellow",
                Brand = "Saab",
                Model = "H88",
                Wheels = 10,
                Arrival = new DateTime(2026, 6, 5, 09, 10, 00),
            },

            new ParkedVehicle()
            {
                Id = 4,
                VehicleType = VehicleType.Motorcycle,
                RegNbr = "TRE654",
                Color = "Black",
                Brand = "Toyota",
                Model = "X76",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 8, 18, 00, 00),
            },

            new ParkedVehicle()
            {
                Id = 5,
                VehicleType = VehicleType.Car,
                RegNbr = "DUN584",
                Color = "Blue",
                Brand = "Saab",
                Model = "C50",
                Wheels = 4,
                Arrival = new DateTime(2026, 7, 1, 10, 45, 00),
            },

            new ParkedVehicle()
            {
                Id = 6,
                VehicleType = VehicleType.Bus,
                RegNbr = "PLG327",
                Color = "White",
                Brand = "Volvo",
                Model = "BG70",
                Wheels = 10,
                Arrival = new DateTime(2026, 6, 28, 14, 50, 00),
            },

            new ParkedVehicle()
            {
                Id = 7,
                VehicleType = VehicleType.Car,
                RegNbr = "NJG968",
                Color = "Black",
                Brand = "Mazda",
                Model = "BT50",
                Wheels = 4,
                Arrival = new DateTime(2026, 6, 30, 16, 25, 00),
            },

            new ParkedVehicle()
            {
                Id = 8,
                VehicleType = VehicleType.Car,
                RegNbr = "RFM596",
                Color = "White",
                Brand = "Toyota",
                Model = "A50",
                Wheels = 4,
                Arrival = new DateTime(2026, 7, 6, 11, 18, 00),
            },

            new ParkedVehicle()
            {
                Id = 9,
                VehicleType = VehicleType.Bus,
                RegNbr = "JYT628",
                Color = "White",
                Brand = "Volvo",
                Model = "AZ34",
                Wheels = 8,
                Arrival = new DateTime(2026, 6, 28, 15, 45, 00),
            },

            new ParkedVehicle()
            {
                Id = 10,
                VehicleType = VehicleType.Motorcycle,
                RegNbr = "DER421",
                Color = "Red",
                Brand = "Toyota",
                Model = "V30",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 8, 10, 18, 00),
            }
            );
    }
}
