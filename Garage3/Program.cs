using Garage3.Data;
using Garage3.Models;
using Garage3.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Garage_3_0Context") ?? throw new InvalidOperationException("Connection string 'Garage_3_0Context' not found.");

builder.Services.AddDbContext<GarageContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUniquenessValidator, UniquenessValidator>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GarageContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

        var db = scope.ServiceProvider.GetRequiredService<GarageContext>();
        db.Database.Migrate();

        await DbInitializer.SeedRolesAsync(services);
        await DbInitializer.SeedAdminAsync(services);
        await DbInitializer.SeedParkingMembers(db, services);

        // Seed test data
        uint nbrParkingSpots = 3;
        await DbInitializer.SeedParkingMembers(db, services);
        await DbInitializer.SeedVehicleTypes(db);
        await DbInitializer.SeedParkingSpots(db, nbrParkingSpots);
        await DbInitializer.SeedParkingSessions(db, services);

        // Add 1 vehicle for test user 1
        // email: test1@test.com
        await DbInitializer.SeedTestVehicle(db, services, "test1@test.com");
    }

app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages()
        .WithStaticAssets();

    app.Run();

