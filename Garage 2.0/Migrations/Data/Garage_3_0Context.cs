using Garage_3._0.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

public class Garage_3_0Context(DbContextOptions<Garage_3_0Context> options) : DbContext(options)
{
    public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;  // TODO - gives error on migration
    public DbSet<VehicleType> VehicleType { get; set; }  // = default!;
    public DbSet<Vehicle> Vehicles { get; set; }  // = default!;
    public DbSet<ParkingSpot> ParkingSpots { get; set; }  // = default!;

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        // Warning!
        // TODO: ERROR - Update-database complains for some "NOT" when running code
        // public DbSet<ParkingSession> ParkingSessions { get; set; }  // = default!;
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ParkedVehicle>().HasIndex(v => v.RegNbr).IsUnique();
        modelBuilder.Entity<ParkedVehicle>().HasData(
            new ParkedVehicle()
            {
                Id = 1,
                VehicleTypes = VehicleTypes.Car,
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
                VehicleTypes = VehicleTypes.Motorcycle,
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
                VehicleTypes = VehicleTypes.Bus,
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
                VehicleTypes = VehicleTypes.Motorcycle,
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
                VehicleTypes = VehicleTypes.Car,
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
                VehicleTypes = VehicleTypes.Bus,
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
                VehicleTypes = VehicleTypes.Car,
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
                VehicleTypes = VehicleTypes.Car,
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
                VehicleTypes = VehicleTypes.Bus,
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
                VehicleTypes = VehicleTypes.Motorcycle,
                RegNbr = "DER421",
                Color = "Red",
                Brand = "Toyota",
                Model = "V30",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 8, 10, 18, 00),
            }
            );



        // Seed VehicleTypes
        // ToDo - change VehicleTypes to VehicleType - temporary name conflict
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<VehicleType>().HasIndex(v => v.Name).IsUnique();
        modelBuilder.Entity<VehicleType>().HasData(
            new VehicleType()
            {
                Id = 1,
                Name = "Bus"
            },
            new VehicleType()
            {
                Id = 2,
                Name = "Car"
            },
            new VehicleType()
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
                VehicleTypeId = 2
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
                VehicleTypeId = 2
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
                VehicleTypeId = 3
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
        // TODO - seed with loop
        //base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<ParkingSession>().HasData(
        //    new ParkingSession()
        //    {
        //        Id = 1,
        //        VehicleId = 1,
        //        ParkingSpotId = 1,
        //        CheckInTime = DateTime.Now,
        //        HourlyRateAtCheckin = 9.9m
        //    },
        //    new ParkingSession()
        //    {
        //        Id = 2,
        //        VehicleId = 2,
        //        ParkingSpotId = 2,
        //        CheckInTime = DateTime.Now,
        //        HourlyRateAtCheckin = 9.9m
        //    },
        //    new ParkingSession()
        //    {
        //        Id = 3,
        //        VehicleId = 3,
        //        ParkingSpotId = 3,
        //        CheckInTime = DateTime.Now,
        //        HourlyRateAtCheckin = 9.9m
        //    }
        //    );

        /* 
         *  Microsoft.EntityFrameworkCore.Model.Validation[30000]
            No store type was specified for the decimal property 'HourlyRateAtCheckin' 
            on entity type 'ParkingSession'. This will cause values to be silently
            truncated if they do not fit in the default precision and scale. 
            Explicitly specify the SQL server column type that can accommodate all the 
            values in 'OnModelCreating' using 'HasColumnType', specify precision and 
            scale using 'HasPrecision', or configure a value converter using 'HasConversion'.
         */


    }
}
