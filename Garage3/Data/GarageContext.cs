using Garage3.Data;
using Garage3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage3.Data;

public class GarageContext(DbContextOptions<GarageContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;  // TODO - ParkedVehicle will be removed later

    // TODO - change name to VehicleType when old VehicleType-model is removed
    public DbSet<VehicleTypeNew> VehicleTypeNew { get; set; }  // = default!; 
    public DbSet<Vehicle> Vehicles { get; set; }  // = default!;
    public DbSet<ParkingSpot> ParkingSpots { get; set; }  // = default!;

    // TODO - this gives error on update-database!
    //public DbSet<ParkingSession> ParkingSession { get; set; }

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
                Arrival = new DateTime(2026, 7, 6, 10, 59, 00),
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


        // Seed VehicleTypes
        // TODO - change VehicleTypes to VehicleType - temporary name conflict
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<VehicleTypeNew>().HasIndex(v => v.Name).IsUnique();
        modelBuilder.Entity<VehicleTypeNew>().HasData(
            new VehicleTypeNew()
            {
                Id = 1,
                Name = "Bus"
            },
            new VehicleTypeNew()
            {
                Id = 2,
                Name = "Car"
            },
            new VehicleTypeNew()
            {
                Id = 3,
                Name = "Motorcycle"
            });

        // Seed Vehicles
        // Todo - add OwnerId
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Vehicle>().HasIndex(v => v.RegistrationNumber).IsUnique();
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle()
            {
                Id = 1,
                RegistrationNumber = "AAA111",
                Color = "Blue",
                Brand = "Alfa Romeo",
                Model = "X99",
                NumberOfWheels = 4,
                VehicleTypeNewId = 2  // TODO - change name to VehicleTypeId when old VehicleType-model is removed
                // OwnerId = ???
            },
            new Vehicle()
            {
                Id = 2,
                RegistrationNumber = "BBB222",
                Color = "Blue",
                Brand = "Ford",
                Model = "Fiesta",
                NumberOfWheels = 4,
                VehicleTypeNewId = 2
                // OwnerId = ???
            },
            new Vehicle()
            {
                Id = 3,
                RegistrationNumber = "CCC333",
                Color = "Red",
                Brand = "Honda",
                Model = "CBX750",
                NumberOfWheels = 2,
                VehicleTypeNewId = 3
                // OwnerId = ???
            }
            );

        // Seed ParkingSpots
        // TODO - seed with loop
        // TODO - how many parking spots?
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ParkingSpot>().HasIndex(v => v.Number).IsUnique();
        modelBuilder.Entity<ParkingSpot>().HasData(
            new ParkingSpot()
            {
                Id = 1,
                Number = 100,
                Location = ""
            },
            new ParkingSpot()
            {
                Id = 2,
                Number = 101,
                Location = ""
            },
            new ParkingSpot()
            {
                Id = 3,
                Number = 102,
                Location = ""
            }
            );

        // Seed ParkingSession
        // TODO - this gives error on update-database!
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ParkingSession>().HasData(
            new ParkingSession()
            {
                Id = 1,
                VehicleId = 1,
                ParkingSpotId = 1,
                CheckInTime = new DateTime(2026, 8, 1),
                HourlyRateAtCheckin = 9.9m
            },
            new ParkingSession()
            {
                Id = 2,
                VehicleId = 2,
                ParkingSpotId = 2,
                CheckInTime = new DateTime(2026, 8, 2),
                HourlyRateAtCheckin = 9.9m
            },
            new ParkingSession()
            {
                Id = 3,
                VehicleId = 3,
                ParkingSpotId = 3,
                CheckInTime = new DateTime(2026, 8, 3),
                HourlyRateAtCheckin = 9.9m
            }
            );

    }
}
