using Garage3.Constants;
using Garage3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage3.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames =
        [
            Roles.Admin,
            Roles.Member
        ];

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(roleName));
            }
        }
    }

    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        const string userEmail = "admin@garage3.local";
        const string userPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(userEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Administrator",
                PersonalIdentityNumber = "19900101-0017",
            };

            var result = await userManager.CreateAsync(
                adminUser,
                userPassword);

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            if (!await userManager.IsInRoleAsync(adminUser, Roles.Admin))
            {
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }


    }

    public static async Task SeedParkingMembers(GarageContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string password = "Tester-1";  // Same for all test users!

        await DbInitializer.SeedMember(
             context, userManager,
             "19900101-0025",
             "test1", "tester1",
             "test1@test.com",
             password);

        await DbInitializer.SeedMember(
             context, userManager,
             "19900101-0033",
             "test2", "tester2",
             "test2@test.com",
             password);
    }

    private static async Task SeedMember(GarageContext context, UserManager<ApplicationUser> userManager, string personalIdentityNumber, string firstName, string lastName, string email, string password = "")
    {
        string pwd = !String.IsNullOrWhiteSpace(password) ? password : "Tester-1";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                PersonalIdentityNumber = personalIdentityNumber,
                EmailConfirmed = true
            };

            var res = await userManager.CreateAsync(user, pwd);

            if (!res.Succeeded)
            {
                throw new Exception(
                    $"Failed to create user: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            }

            if (!await userManager.IsInRoleAsync(user, Roles.Member))
            {
                await userManager.AddToRoleAsync(user, Roles.Member);
            }
        }
    }

    public static async Task SeedVehicleTypes(GarageContext context)
    {
        // Vehicle types
        ICollection<string> typeList = ["Bus", "Car", "Motorcycle"];

        var types = await context.VehicleTypeNew.FirstOrDefaultAsync();

        if (types == null)
        {
            foreach (string type in typeList)
            {
                var vt = new VehicleTypeNew() { Name = type };
                context.Add(vt);
                await context.SaveChangesAsync();
            }
        }
    }

    public static async Task SeedParkingSpots(GarageContext context, uint nbrParkingSpots)
    {
        var parkingspot = await context.ParkingSpots.FirstOrDefaultAsync();

        if (parkingspot == null)
        {
            for (int i = 0; i < nbrParkingSpots; i++)
            {
                var spot = new ParkingSpot()
                {
                    Number = 100 + i,
                    Location = ""
                };
                context.Add(spot);
            }
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedParkingSessions(GarageContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var member = await userManager.FindByEmailAsync("test1@test.com");

        if (member == null) return;

        if (!await context.Vehicles.AnyAsync())
        {
            context.Vehicles.Add(new Vehicle
            {
                VehicleTypeId = 2,
                VehicleTypeNewId = 2,
                RegNbr = "ABC123",
                Color = "Red",
                Brand = "Volvo",
                Model = "V60",
                NumberOfWheels = 4,
                //Arrival = new DateTime(2026, 7, 6, 10, 59, 00),
                ApplicationUserId = member.Id
            });

            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedParkingSessionsToParkingSpot(GarageContext context, IServiceProvider serviceProvider)
    {
        //var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //var member = await userManager.FindByEmailAsync("test1@test.com");
        //if (member == null) return;

        // Vehicle exists
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.RegNbr == "NNN111");
        if (vehicle == null) return;

        // Check that the vehcle isn't already parked
        bool vehicleAlreadyParked = await context.ParkingSession
            .AnyAsync(ps => ps.VehicleId == vehicle.Id && ps.CheckOutTime == null);

        if (vehicleAlreadyParked) return;
        // TempData["ErrorMessage"] "Error - the vehicle is already parked!"
        // => MyVehicle

        // Find first available spot - not out of service and not occupied (CheckOutTime == null)
        var spot = await context.ParkingSpots
            .Where(s => !s.IsOutOfService &&
                        !context.ParkingSession.Any(ps => ps.ParkingSpotId == s.Id && ps.CheckOutTime == null))
            .OrderBy(s => s.Number)
            .FirstOrDefaultAsync();

        if (spot == null) return;
        // TempData["ErrorMessage"] "Parking is already full!"
        // => MyVehicle

        var session = new ParkingSession
        {
            VehicleId = vehicle.Id,
            ParkingSpotId = spot.Id,
            CheckInTime = DateTime.Now,
            HourlyRateAtCheckin = 10m // TODO - where to set price per hour?
        };

        // TempData["Success"] = "Vehicle has been successfully parked!";
        context.ParkingSession.Add(session);
        await context.SaveChangesAsync();
    }

    public static async Task SeedTestVehicle(GarageContext context, IServiceProvider serviceProvider, string email)
    {
        // Add a car for first test user
        string regNbr = "NNN111";
        string vehicleType = "Car";

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByEmailAsync(email);

        if (user != null)
        {
            var tmpVehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.RegNbr == regNbr);

            if (tmpVehicle == null)
            {
                VehicleTypeNew? type = await context.VehicleTypeNew.FirstOrDefaultAsync(t => t.Name == vehicleType);
                if (type != null)
                {
                    Vehicle vehicle = new Vehicle()
                    {
                        RegNbr = regNbr, // nbr.ToString(),
                        Color = "Black",
                        Brand = "SAAB",
                        Model = "900",
                        NumberOfWheels = 4,
                        VehicleTypeId = type.Id,
                        VehicleTypeNewId = type.Id,
                        ApplicationUser = user,
                        ApplicationUserId = user.Id
                    };
                    context.Add(vehicle);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}