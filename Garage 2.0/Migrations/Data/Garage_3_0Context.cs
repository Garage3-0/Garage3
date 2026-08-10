using Garage_3._0.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

public class Garage_3_0Context(DbContextOptions<Garage_3_0Context> options) : DbContext(options)
{
    public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;  // TODO - gives error on migration
    public DbSet<VehicleType> VehicleTypes { get; set; }  // = default!;
    public DbSet<Vehicle> Vehicles { get; set; }  // = default!;
    public DbSet<ParkingSpot> ParkingSpots { get; set; }  // = default!;

        // TODO: ERROR - Update-database complains for some "NOT" when running code
        // public DbSet<ParkingSession> ParkingSessions { get; set; }  // = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed VehicleTypes
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
