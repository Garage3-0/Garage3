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
    [Authorize]
    public async Task<IActionResult> Index(string searchRegNbr)
    {
        ViewData["CurrentFilter"] = searchRegNbr;

        var currentUserId = _userManager.GetUserId(User);
        bool isAdmin = User.IsInRole("Admin");

        var vehicles = _context.ParkedVehicle
            .Include(v => v.VehicleType)
            .AsQueryable();

        if (!isAdmin)
        {
            vehicles = vehicles.Where(v => v.ApplicationUserId == currentUserId);
        }

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
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "A valid vehicle ID must be provided.";
            return RedirectToAction(nameof(MyVehicles));
        }

        var currentUserId = _userManager.GetUserId(User);
        bool isAdmin = User.IsInRole("Admin");

        var parkedvehicle = await _context.ParkedVehicle
            .Include(v => v.VehicleType)
        .FirstOrDefaultAsync(m => m.Id == id && (isAdmin || m.ApplicationUserId == currentUserId));

        if (parkedvehicle == null)
        {
            TempData["ErrorMessage"] = "Vehicle not found or you do not have permission to view it.";
            return RedirectToAction(nameof(MyVehicles));
        }

        return View(parkedvehicle);
    }

    // GET: PARKEDVEHICLES/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        var availableVehicles = await GetAvailableVehiclesForCurrentUserAsync();

        // Task 71 - get parking spots that are available
        var availableParkingSpots = await GetAvailableParkingSpotsAsync();

        var model = new ParkVehicleViewModel
        {
            VehicleTypes = new SelectList(
                await _context.VehicleTypeNew.ToListAsync(),
                "Id",
                "Name"),

            Vehicles = new SelectList(
                availableVehicles,
                "Id",
                "RegistrationNumber"),

            ParkingSpots = new SelectList(
                availableParkingSpots,
                "Id",
                "Number")
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
        string? error = await _uniquenessValidator.IsRegNbrUniqueAsync(viewModel.RegNbr);
        if (error != null)
        {
            ModelState.AddModelError(nameof(viewModel.RegNbr), error);
        }

        if (ModelState.IsValid)
        {
            var normalizedRegNbr = viewModel.RegNbr!.Trim().ToUpperInvariant();
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
                var innerMessage = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                ModelState.AddModelError(
                    string.Empty,
                    $"Database error: {innerMessage}");
            }
        }

        viewModel.VehicleTypes = new SelectList(
            await _context.VehicleTypeNew.ToListAsync(),
            "Id",
            "Name");

        TempData["Error"] = "Vehicle could not be parked!";
        return View(viewModel);
    }

    // GET: PARKEDVEHICLES/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "A valid vehicle ID must be provided to edit.";
            return RedirectToAction(nameof(MyVehicles));
        }

        var currentUserId = _userManager.GetUserId(User);
        bool isAdmin = User.IsInRole("Admin");

        var parkedvehicle = await _context.ParkedVehicle.
            FirstOrDefaultAsync(v => v.Id == id && (isAdmin || v.ApplicationUserId ==currentUserId));


        if (parkedvehicle == null)
        {
            TempData["ErrorMessage"] = "Vehicle not found or you do not have permission to edit it.";
            return RedirectToAction(nameof(MyVehicles));
        }

        var viewModel = new ParkedVehicleEditViewModel
        {
            Id = parkedvehicle.Id,
            VehicleTypeId = parkedvehicle.VehicleTypeId,
            VehicleTypes = new SelectList(
                await _context.VehicleTypeNew.ToListAsync(),
                "Id",
                "Name",
                parkedvehicle.VehicleTypeId),

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
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id, ParkedVehicleEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ErrorMessage"] = "Data mismatch error. The request could not be processed.";
            return RedirectToAction(nameof(Index));
        }

        string? error = await _uniquenessValidator.IsRegNbrUniqueAsync(viewModel.RegNbr, viewModel.Id);
        if (error != null)
        {
            ModelState.AddModelError(nameof(viewModel.RegNbr), error);
        }

        if (ModelState.IsValid)
        {
          var currentUserId = _userManager.GetUserId(User);
          bool isAdmin = User.IsInRole("Admin");

          var parkedvehicle = await _context.ParkedVehicle
            .FirstOrDefaultAsync(v => v.Id == id && (isAdmin || v.ApplicationUserId == currentUserId));


          if (parkedvehicle == null)
          {
            TempData["ErrorMessage"] = "Vehicle not found or you do not have permission to update it.";
            return RedirectToAction(nameof(Index));
          }
          
          try
            {
                parkedvehicle.VehicleTypeId = viewModel.VehicleTypeId;
                parkedvehicle.RegNbr = viewModel.RegNbr!.Trim().ToUpperInvariant();
                parkedvehicle.Color = viewModel.Color;
                parkedvehicle.Brand = viewModel.Brand;
                parkedvehicle.Model = viewModel.Model;
                parkedvehicle.Wheels = viewModel.Wheels;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Vehicle with registration number \"{parkedvehicle.RegNbr}\" has been updated!";
                return isAdmin ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(MyVehicles));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(viewModel.Id))
                {
                    TempData["ErrorMessage"] = "The vehicle was removed by another user during the process.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "A database concurrency error occurred. Please try again.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        viewModel.VehicleTypes = new SelectList(await _context.VehicleTypeNew.ToListAsync(), "Id", "Name", viewModel.VehicleTypeId);
        TempData["Error"] = "Failed to update vehicle details. Please check the inputs.";
    
        return View(viewModel);
  }

    private bool ParkedVehicleExists(int id)
    {
        var currentUserId = _userManager.GetUserId(User);

        return _context.ParkedVehicle.Any(e =>
            e.Id == id &&
            e.ApplicationUserId == currentUserId);
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
        var tmp = TempData["receipt"] as string ?? "";

        if (!string.IsNullOrWhiteSpace(tmp))
        {
            try
            {
                ReceiptViewModel? receiptViewModel =
                    JsonSerializer.Deserialize<ReceiptViewModel>(tmp);

                return View(receiptViewModel);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }

        TempData["Success"] = null;
        TempData["ErrorMessage"] =
            "Technical error - the vehicle is checked out but we failed to show receipt!";

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
        var currentUserId = _userManager.GetUserId(User);

        var myVehicles = await _context.ParkedVehicle
            .Where(v => v.ApplicationUserId == currentUserId)
            .Select(v => new ParkedVehicleOverviewViewModel
            {
                Id = v.Id,
                VehicleTypeId = v.VehicleTypeId,
                VehicleTypeName = v.VehicleType != null
                    ? v.VehicleType.Name
                    : "N/A",

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

    // Task 70 - get the logged in user's vehicles that are not already parked
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
                !v.ParkingSessions.Any(ps =>
                    ps.CheckOutTime == null))
            .OrderBy(v => v.RegistrationNumber)
            .ToListAsync();

        return vehicles;
    }

    // Task 71 - get parking spots without an active parking session
    private async Task<List<ParkingSpot>> GetAvailableParkingSpotsAsync(
        string? location = null)
    {
        var query = _context.ParkingSpots
            .Where(p =>
                !p.IsOutOfService &&
                !_context.ParkingSession.Any(ps =>
                    ps.ParkingSpotId == p.Id &&
                    ps.CheckOutTime == null));

        // Only one location is currently used.
        // The filter remains available if a location is supplied.
        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(p =>
                p.Location == location);
        }

        return await query
            .OrderBy(p => p.Number)
            .ToListAsync();
    }

    // GET: ParkedVehicles/Register
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Register()
    {
        var viewModel = new RegisterVehicleViewModel
        {
            VehicleTypes = new SelectList(
                await _context.VehicleTypeNew.ToListAsync(),
                "Id",
                "Name")
        };

        return View(viewModel);
    }

    // POST: ParkedVehicles/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Register(
        RegisterVehicleViewModel viewModel)
    {
        string? error = await _uniquenessValidator.IsRegNbrUniqueAsync(viewModel.RegNbr);
        if (error != null)
        {
            ModelState.AddModelError(nameof(viewModel.RegNbr), error);
        }

        if (ModelState.IsValid)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var normalizedRegNbr = viewModel.RegNbr?.Trim().ToUpperInvariant() ?? string.Empty;
            var vehicle = new ParkedVehicle 
                {
                    VehicleTypeId = viewModel.VehicleTypeId,
                    RegNbr = normalizedRegNbr,
                    Color = viewModel.Color ?? string.Empty,
                    Brand = viewModel.Brand ?? string.Empty,
                    Model = viewModel.Model ?? string.Empty,
                    Wheels = viewModel.Wheels,
                    ApplicationUserId = currentUser.Id
                };

                _context.Add(vehicle);
                await _context.SaveChangesAsync();

                TempData["Success"] =
                    $"Vehicle {normalizedRegNbr} was successfully registered!";

                return RedirectToAction(nameof(MyVehicles));
            }

        viewModel.VehicleTypes = new SelectList(
            await _context.VehicleTypeNew.ToListAsync(),
            "Id",
            "Name");

        return View(viewModel);
    }
}