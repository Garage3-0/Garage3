using Garage3.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Garage3.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAsync(
        IServiceProvider serviceProvider)
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
        const string userPassword = "Testare-1";

        var memberUser = await userManager.FindByEmailAsync(userEmail);

        if (memberUser == null)
        {
            memberUser = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(
                memberUser,
                userPassword);

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Failed to create member user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            if (!await userManager.IsInRoleAsync(memberUser, Roles.Admin))
            {
                await userManager.AddToRoleAsync(
                    memberUser,
                    Roles.Admin);
            }
        }

        
    }

    //--------------------------------------------------------
    // (ApplicationDbContext _context, IServiceProvider services)
    public static async Task SeedParkingMembers(GarageContext context, IServiceProvider serviceProvider)
    {

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


        var userMail = "u2@u.com";

        var user = await userManager.FindByEmailAsync(userMail);

        if (user == null) {
            user = new ApplicationUser
            {
                FirstName = "u2",
                LastName = "user2",
                Email = "u2@u.com",
                NormalizedEmail = "U2@U.COM",
                UserName = "u2",
                NormalizedUserName = "U1",
                PersonalIdentityNumber = "1",
                //PhoneNumber = "+111111111111",
                EmailConfirmed = false
                //,
                //PhoneNumberConfirmed = true,
                //SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var res = await userManager.CreateAsync(
                    user,
                    "Testare-1");

            if (!res.Succeeded)
            {
                throw new Exception(
                    $"Failed to create user user: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            }

             if (!await userManager.IsInRoleAsync(user, Roles.Member))
            {
                await userManager.AddToRoleAsync(
                    user,
                    Roles.Member);
            }
            // AssignRoles(serviceProvider, user.Email, roles);

            await context.SaveChangesAsync();
        }
    }

    //public static async Task SeedParkingSessions(GarageContext garage, IServiceProvider serviceProvider)
    //{

    //}

    //public static async Task SeedParkingSpotsAsync(GarageContext _context, IServiceProvider serviceProvider, int count)
    //{
    //    count parkingSpots


    //}
}