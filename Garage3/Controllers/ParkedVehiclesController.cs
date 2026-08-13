using Garage3.Data;
using Garage3.Models;
using Garage3.Models.ViewModels;
using Garage3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

[Authorize]
public class ParkedVehiclesController : Controller
{
    const int pricePerHour = 10;

    private readonly GarageContext _context;
    private readonly IUniquenessValidator _uniquenessValidator;
    private readonly UserManager<ApplicationUser> _userManager;

    public ParkedVehiclesController(GarageContext context, IUniquenessValidator uniquenessValidator, UserManager<ApplicationUser> userManager)

    {
        _context = context;
        _uniquenessValidator = uniquenessValidator;
        _userManager = userManager;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index(string searchRegNbr)
    {
        ViewData["CurrentFilter"] = searchRegNbr;

        var vehicles = _context.ParkedVehicle.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchRegNbr))
        {
            var searchResults = vehicles.Where(v => v.RegNbr.Contains(searchRegNbr));

            if (searchResults.Any())
            {
                ViewData["Exists"] = true;
                vehicles = searchResults;
            }
            else
            {
                ViewData["Exists"] = false;
            }
        }
        var model = await vehicles.Select(v => new ParkedVehicleOverviewViewModel
        {
            Id = v.Id,
            VehicleTypeId = v.VehicleTypeId,
            VehicleTypeName = v.VehicleType != null ? v.VehicleType.Name : "Unknown",
            RegNbr = v.RegNbr,
            Color = v.Color,
            Brand = v.Brand,
            Model = v.Model,
            Wheels = v.Wheels,
            Arrival = v.Arrival

        })
        .ToListAsync();

        return View(model);
    }
    // GET: PARKEDVEHICLES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var currentUserId = _userManager.GetUserId(User);

        var parkedvehicle = await _context.ParkedVehicle
            .FirstOrDefaultAsync(m => m.Id == id && m.ApplicationUserId == currentUserId);

        if (parkedvehicle == null)
        {
            TempData["ErrorMessage"] = "You do not have permission to view this vehicle.";
            return RedirectToAction(nameof(MyVehicles));
        }

        return View(parkedvehicle);
    }

    // GET: PARKEDVEHICLES/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        var availableVehicles = await GetAvailableVehiclesForCurrentUserAsync();

        var model = new ParkVehicleViewModel
        {
            VehicleTypes = new SelectList(
                await _context.VehicleTypeNew.ToListAsync(),
                "Id",
                "Name"),

            Vehicles = new SelectList(
                availableVehicles,
                "Id",
                "RegistrationNumber")
        };

        return View(model);
    }

    // POST: PARKEDVEHICLES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(ParkVehicleViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var normalizedRegNbr = viewModel.RegNbr?.Trim().ToUpper() ?? string.Empty;

            bool isUnique = await _uniquenessValidator.IsRegNbrUniqueAsync(normalizedRegNbr);

            if (!isUnique)
            {
                ModelState.AddModelError("RegNbr", "This registration number already exists in the garage. There can only be one vehicle per registration number.");
                viewModel.VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name");
                return View(viewModel);
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                TempData["Error"] = "Your session is invalid or user was not found. Please log in again.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var parkedVehicle = new ParkedVehicle
            {
                VehicleTypeId = viewModel.VehicleTypeId,
                RegNbr = normalizedRegNbr,
                Color = viewModel.Color ?? string.Empty,
                Brand = viewModel.Brand ?? string.Empty,
                Model = viewModel.Model ?? string.Empty,
                Wheels = viewModel.Wheels,
                Arrival = DateTime.Now,
                ApplicationUserId = currentUser.Id
            };

            try
            {
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Vehicle has been successfully parked!";
                return RedirectToAction(nameof(MyVehicles));
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError(string.Empty, $"Database error: {innerMessage}");
            }
        }
        viewModel.VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name");
        TempData["Error"] = "Vehicle could not be parked!";
        return View(viewModel);
    }


    // GET: PARKEDVEHICLES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "A valid vehicle ID must be provided to edit.";
            return RedirectToAction(nameof(MyVehicles));
        }

        var currentUserId = _userManager.GetUserId(User);

        var parkedvehicle = await _context.ParkedVehicle.
            FirstOrDefaultAsync(v => v.Id == id && v.ApplicationUserId == currentUserId);

        if (parkedvehicle == null)
        {
            TempData["ErrorMessage"] = "Vehicle not found or you do not have permission to edit it.";
            return RedirectToAction(nameof(MyVehicles));
        }

        var viewModel = new ParkedVehicleEditViewModel
        {
            Id = parkedvehicle.Id,
            VehicleTypeId = parkedvehicle.VehicleTypeId,
            VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name", parkedvehicle.VehicleTypeId),
            RegNbr = parkedvehicle.RegNbr,
            Color = parkedvehicle.Color,
            Brand = parkedvehicle.Brand,
            Model = parkedvehicle.Model,
            Wheels = parkedvehicle.Wheels,
            Arrival = parkedvehicle.Arrival
        };

        return View(viewModel);
    }

    // POST: PARKEDVEHICLES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ParkedVehicleEditViewModel viewModel)
    {
        // If the ID in the URL does not match the ID in the form
        if (id != viewModel.Id)
        {
            TempData["ErrorMessage"] = "Data mismatch error. The request could not be processed.";
            return RedirectToAction(nameof(MyVehicles));
        }

        if (ModelState.IsValid)
        {
            try
            {
                var normalizedRegNbr = viewModel.RegNbr.Trim().ToUpper() ?? string.Empty;
                // Is there another vehicle that already has this reg number?
                bool isUnique = await _uniquenessValidator
                    .IsRegNbrUniqueAsync(normalizedRegNbr, viewModel.Id);

                if (!isUnique)
                {
                    ModelState.AddModelError("RegNbr", "This registration number is already occupied by another parked vehicle.");
                    viewModel.VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name", viewModel.VehicleTypeId);
                    return View(viewModel); // "Abort" and send user back to view with error message
                }

                var currentUserId = _userManager.GetUserId(User);

                var parkedvehicle = await _context.ParkedVehicle.
                    FirstOrDefaultAsync(v => v.Id == id && v.ApplicationUserId == currentUserId);

                // If the vehicle has been removed from the database while another user edited it
                if (parkedvehicle == null)
                {
                    TempData["ErrorMessage"] = "Vehicle not found or you do not have permission to update it.";
                    return RedirectToAction(nameof(MyVehicles));
                }

                parkedvehicle.VehicleTypeId = viewModel.VehicleTypeId;
                parkedvehicle.RegNbr = normalizedRegNbr;
                parkedvehicle.Color = viewModel.Color;
                parkedvehicle.Brand = viewModel.Brand;
                parkedvehicle.Model = viewModel.Model;
                parkedvehicle.Wheels = viewModel.Wheels;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Vehicle with registration number \"{parkedvehicle.RegNbr}\" has been updated!";
                return RedirectToAction(nameof(MyVehicles));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(viewModel.Id))
                {
                    TempData["ErrorMessage"] = "The vehicle was removed by another user during the process.";
                    return RedirectToAction(nameof(MyVehicles));
                }
                else
                {
                    TempData["ErrorMessage"] = "A database concurrency error occurred. Please try again.";
                    return RedirectToAction(nameof(MyVehicles));
                }
            }

            //TempData["Success"] = "Vehicle details updated successfully!";
            //return RedirectToAction(nameof(Index));
        }
        viewModel.VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name", viewModel.VehicleTypeId);
        TempData["Error"] = "Failed to update vehicle details.";
        return View(viewModel);
    }

    private bool ParkedVehicleExists(int id)
    {
        var currentUserId = _userManager.GetUserId(User);
        return _context.ParkedVehicle.Any(e => e.Id == id && e.ApplicationUserId == currentUserId);
    }

    // GET: PARKEDVEHICLES/Delete/5
    // [Authorize]
    [Authorize(Roles = "Admin, Member")]
    public async Task<IActionResult> Checkout(int? id)
    {
        if (id == null)
        {
            //return NotFound();
            TempData["ErrorMessage"] = "Technical error";
            return RedirectToAction(nameof(MyVehicles));
        }

        // Get vehicle, type and owner
        var vehicle = await _context.Vehicles
            .Where(v => v.Id == id)
            .Include(v => v.VehicleTypeNew)
            .Include(v => v.ApplicationUser)
            .FirstOrDefaultAsync();

        if (vehicle == null)
        {
            TempData["ErrorMessage"] = "Sorry, we can't find the vehicle - wrong vehicle id";
            return RedirectToAction(nameof(MyVehicles));
        }

        // Check owner/Admin
        var currentUserId = _userManager.GetUserId(User);
        if (!User.IsInRole("Admin") && vehicle.ApplicationUserId != currentUserId)
        {
            TempData["ErrorMessage"] = "You are not allowed to checkout this vehicle.";
            return RedirectToAction(nameof(MyVehicles));
        }

        // Get parking session and parking spot
        var activeSession = await _context.ParkingSession
            .Where(ps => ps.VehicleId == vehicle.Id && ps.CheckOutTime == null)
            .Include(ps => ps.ParkingSpot)
            .FirstOrDefaultAsync();

        if (activeSession == null)
        {
            TempData["ErrorMessage"] = "The vehicle is not parked!";
            return RedirectToAction(nameof(MyVehicles));
        }

        ReceiptViewModel receiptViewModel = CreateReceiptViewModelFromVehicleAndSession(vehicle, activeSession);

        return View(receiptViewModel);
    }

    private ReceiptViewModel CreateReceiptViewModelFromVehicleAndSession(Vehicle vehicle, ParkingSession session)
    {
        DateTime arrival = session.CheckInTime;
        DateTime checkout = DateTime.Now;
        TimeSpan totalTime = checkout - arrival;

        int days = totalTime.Days;
        int hours = totalTime.Hours;
        int minutes = totalTime.Minutes;

        decimal rate = session.HourlyRateAtCheckin;
        decimal totalPriceDecimal = (days * 24m * rate) + (hours * rate) + (minutes * rate / 60m);
        int totalPrice = (int)Math.Ceiling(totalPriceDecimal);

        string fullName = vehicle.ApplicationUser != null
            ? $"{vehicle.ApplicationUser.FirstName} {vehicle.ApplicationUser.LastName}"
            : string.Empty;

        return new ReceiptViewModel
        {
            Id = session.Id,
            VehicleTypeId = vehicle.VehicleTypeNewId,
            VehicleTypeName = vehicle.VehicleTypeNew?.Name,
            RegNbr = vehicle.RegistrationNumber,
            Color = vehicle.Color,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Wheels = vehicle.NumberOfWheels,
            Arrival = arrival,
            CheckoutTime = checkout,
            ParkedDays = days,
            ParkedHours = hours,
            ParkedMinutes = minutes,
            Price = totalPrice,
            PricePerHour = (int)Math.Ceiling(rate),  // Round to int
            FullName = fullName,
            ParkingSpot = session.ParkingSpot?.Number ?? 0
        };
    }

    //GET: PARKEDVEHICLES/Receipt/5
    public async Task<IActionResult> Receipt()
    {
        //  Gets time and price info via TempData
        var tmp = TempData["receipt"] as string ?? "";  // Bad spelling
        if (!string.IsNullOrWhiteSpace(tmp))
        {
            try
            {
                ReceiptViewModel? receiptViewModel = JsonSerializer.Deserialize<ReceiptViewModel>(tmp);
                return View(receiptViewModel);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw; // Error...
            }
        }

        // If error...
        TempData["Success"] = null;  // Remove success text
        TempData["ErrorMessage"] = "Technical error - the vehicle is checked out but we failed to show receipt!";

        return RedirectToAction(nameof(Index));
    }

    // POST: PARKEDVEHICLES/Receipt/5
    [HttpPost, ActionName("Receipt")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "Sorry, we can't find the vehicle!";
            return RedirectToAction(nameof(MyVehicles));
        }

        // Get vehicle, type and owner
        var vehicle = await _context.Vehicles
            .Where(v => v.Id == id)
            .Include(v => v.VehicleTypeNew)
            .Include(v => v.ApplicationUser)
            .FirstOrDefaultAsync();

        if (vehicle == null)
        {
            return NotFound();
        }

        // Check owner/Admin
        var currentUserId = _userManager.GetUserId(User);
        if (!User.IsInRole("Admin") && vehicle.ApplicationUserId != currentUserId)
        {
            TempData["ErrorMessage"] = "You are not allowed to checkout this vehicle.";
            return RedirectToAction(nameof(MyVehicles));
        }

        // Get parking session and parking spot
        var activeSession = await _context.ParkingSession
            .Where(ps => ps.VehicleId == vehicle.Id && ps.CheckOutTime == null)
            .Include(ps => ps.ParkingSpot)
            .FirstOrDefaultAsync();

        if (activeSession == null)
        {
            TempData["ErrorMessage"] = "The vehicle is not parked!";
            return RedirectToAction(nameof(MyVehicles));
        }

        //Store data for receipt in TempData
        ReceiptViewModel receiptViewModel = CreateReceiptViewModelFromVehicleAndSession(vehicle, activeSession);
        TempData["receipt"] = JsonSerializer.Serialize(receiptViewModel);

        // Remve vehicle from ParkingSession
        activeSession.CheckOutTime = receiptViewModel.CheckoutTime;
        await _context.SaveChangesAsync();

        TempData["Success"] = "Vehicle has been checked out.";

        return RedirectToAction("Receipt", "ParkedVehicles");
    }

    // GET: PARKEDVEHICLES/MyVehicles
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> MyVehicles()
    {
        // Get ID for logged in user
        var currentUserId = _userManager.GetUserId(User);

        // Filter for vehicles with the same ApplicationUserId
        var myVehicles = await _context.ParkedVehicle
            .Where(v => v.ApplicationUserId == currentUserId)
            .Select(v => new ParkedVehicleOverviewViewModel
            {
                Id = v.Id,
                VehicleTypeId = v.VehicleTypeId,
                VehicleTypeName = v.VehicleType != null ? v.VehicleType.Name : "N/A",
                RegNbr = v.RegNbr,
                Color = v.Color,
                Brand = v.Brand,
                Model = v.Model,
                Wheels = v.Wheels,
                Arrival = v.Arrival
            })
            .ToListAsync();

        return View(myVehicles);
    }
    private async Task<List<Vehicle>> GetAvailableVehiclesForCurrentUserAsync()
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId == null)
        {
            return new List<Vehicle>();
        }

        var vehicles = await _context.Vehicles
            .Where(v =>
                v.ApplicationUserId == currentUserId &&
                !v.ParkingSessions.Any(ps => ps.CheckOutTime == null))
            .OrderBy(v => v.RegistrationNumber)
            .ToListAsync();

        return vehicles;
    }

    // GET: ParkedVehicles/Register
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Register()
    {
        var viewModel = new RegisterVehicleViewModel
        {
            VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name")
        };
        return View(viewModel);
    }

    // POST: ParkedVehicles/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Register(RegisterVehicleViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var normalizedRegNbr = viewModel.RegNbr?.Trim().ToUpper() ?? string.Empty;

            if (!await _uniquenessValidator.IsRegNbrUniqueAsync(normalizedRegNbr))
            {
                ModelState.AddModelError("RegNbr", "This registration number is already registered.");
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }


                var vehicle = new ParkedVehicle
                {
                    VehicleTypeId = viewModel.VehicleTypeId,
                    RegNbr = normalizedRegNbr,
                    Color = viewModel.Color,
                    Brand = viewModel.Brand,
                    Model = viewModel.Model,
                    Wheels = viewModel.Wheels,
                    ApplicationUserId = currentUser.Id
                };

                _context.Add(vehicle);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Vehicle {normalizedRegNbr} was successfully registered!";
                return RedirectToAction(nameof(MyVehicles));
            }
        }

        viewModel.VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name");
        return View(viewModel);
    }
}
