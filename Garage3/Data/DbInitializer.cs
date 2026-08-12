using Garage3.Constants;
using Garage3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

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

        const string userEmail = "u2@garage3.local";
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
                PersonalIdentityNumber = "198001010000"
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

        await SeedMember(context, userManager, "test1", "tester1", "test1@test.com", "199001011234");
        await SeedMember(context, userManager, "test2", "tester2", "test2@test.com", "199001025678");
    }

    public static async Task SeedMember(GarageContext context, UserManager<ApplicationUser> userManager,
        string firstName,
        string lastName,
        string email,
        string personalNbr,
        string password = "")
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
                PersonalIdentityNumber = personalNbr,
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

        if (!await context.ParkedVehicle.AnyAsync())
        {
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegNbr = "ABC123",
                Color = "Red",
                Brand = "Volvo",
                Model = "V60",
                Wheels = 4,
                Arrival = new DateTime(2026, 7, 6, 10, 59, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Motorcycle,
                RegNbr = "BGD567",
                Color = "Blue",
                Brand = "Toyota",
                Model = "A50",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 4, 11, 59, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Bus,
                RegNbr = "KLI908",
                Color = "Yellow",
                Brand = "Saab",
                Model = "H88",
                Wheels = 10,
                Arrival = new DateTime(2026, 6, 5, 09, 10, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Motorcycle,
                RegNbr = "TRE654",
                Color = "Black",
                Brand = "Toyota",
                Model = "X76",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 8, 18, 00, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegNbr = "DUN584",
                Color = "Blue",
                Brand = "Saab",
                Model = "C50",
                Wheels = 4,
                Arrival = new DateTime(2026, 7, 1, 10, 45, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Bus,
                RegNbr = "PLG327",
                Color = "White",
                Brand = "Volvo",
                Model = "BG70",
                Wheels = 10,
                Arrival = new DateTime(2026, 6, 28, 14, 50, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegNbr = "NJG968",
                Color = "Black",
                Brand = "Mazda",
                Model = "BT50",
                Wheels = 4,
                Arrival = new DateTime(2026, 6, 30, 16, 25, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegNbr = "RFM596",
                Color = "White",
                Brand = "Toyota",
                Model = "A50",
                Wheels = 4,
                Arrival = new DateTime(2026, 7, 6, 11, 18, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Bus,
                RegNbr = "JYT628",
                Color = "White",
                Brand = "Volvo",
                Model = "AZ34",
                Wheels = 8,
                Arrival = new DateTime(2026, 6, 28, 15, 45, 00),
                ApplicationUserId = member.Id
            });
            context.ParkedVehicle.Add(new ParkedVehicle
            {
                VehicleType = VehicleType.Motorcycle,
                RegNbr = "DER421",
                Color = "Red",
                Brand = "Toyota",
                Model = "V30",
                Wheels = 2,
                Arrival = new DateTime(2026, 7, 8, 10, 18, 00),
                ApplicationUserId = member.Id
            });

            await context.SaveChangesAsync();
        }
    }
}

//var vehicleType = await context.VehicleTypeNew.FirstOrDefaultAsync();
//var spot = await context.ParkingSpots.FirstOrDefaultAsync();

//if (member == null || vehicleType == null || spot == null) return;

//if (!await context.ParkedVehicle.AnyAsync()) 
//{
//var vehicle = new ParkedVehicle
//{
// RegistrationNumber = "ABC123",
// Använd det riktiga GUID-ID:t från den hämtade användaren:
//ApplicationUserId = member.Id, 
//VehicleTypeNewId = vehicleType.Id,
//ParkingSpotId = spot.Id
//};


//    // User
//    // Vehicle
//    // Parkignspot
//    // ParkingSession
//}

