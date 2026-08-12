using Garage3.Data;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Garage_3_0Context") ?? throw new InvalidOperationException("Connection string 'Garage_3_0Context' not found.");

// Get hourlyRate from appsettings.json
var priceData = builder.Configuration.GetSection("PriceData");
var tmp = priceData.GetSection("HourlyRate");
//public const decimal hourlyRate = decimal.TryParse(tmp.Value, out decimal r) ? r : default;

//public static readonly decimal hourlyRate =
//    decimal.TryParse(tmp.Value, System.Globalization.NumberStyles.Number,
//                     System.Globalization.CultureInfo.InvariantCulture, out decimal r) ? r : default;

var hourlyRate = builder.Configuration.GetValue<decimal>("PriceData:HourlyRate");



//var t = priceData["HourlyRate"];

//var price = priceData.Hour
//Console.WriteLine(settings2["Settings2"]);

builder.Services.AddDbContext<GarageContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<GarageContext>();

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

    // Seed test data
    uint nbrParkingSpots = 3;
    await DbInitializer.SeedParkingMembers(db, services);
    await DbInitializer.SeedVehicleTypes(db);
    await DbInitializer.SeedParkingSpots(db, nbrParkingSpots);
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
