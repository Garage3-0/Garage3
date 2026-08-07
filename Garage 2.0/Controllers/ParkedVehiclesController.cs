
using Garage_3._0.Models;
using Garage_3._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

public class ParkedVehiclesController : Controller
{
    const int pricePerHour = 10;

    private readonly Garage_3_0Context _context;

    public ParkedVehiclesController(Garage_3_0Context context)
    {
        _context = context;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index()
    {
        var vehicles = await _context.ParkedVehicle
            .Select(v => new ParkedVehicleOverviewViewModel
            {
                Id = v.Id,
                VehicleType = v.VehicleType,
                RegNbr = v.RegNbr,
                Color = v.Color,
                Brand = v.Brand,
                Model = v.Model,
                Wheels = v.Wheels,
                Arrival = v.Arrival

            })
            .ToListAsync();

        return View(vehicles);
    }
    // GET: PARKEDVEHICLES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var parkedvehicle = await _context.ParkedVehicle
            .FirstOrDefaultAsync(m => m.Id == id);
        if (parkedvehicle == null)
        {
            return NotFound();
        }

        return View(parkedvehicle);
    }

    // GET: PARKEDVEHICLES/Search (IndexWithViewModel)
    public async Task<IActionResult> Search(string searchRegNbr)
    {
        // Starting point: all vehicles in the database
        var vehiclesQuery = _context.ParkedVehicle.AsQueryable();
        bool? exists = null;

        // IF the user has written something in the search field
        if (!string.IsNullOrEmpty(searchRegNbr))
        {
            searchRegNbr = searchRegNbr.Trim().ToUpper();
            ViewData["CurrentFilter"] = searchRegNbr; // Saves the text in the search field

            // Does the exact reg number exist?
            exists = _context.ParkedVehicle.Any(v => v.RegNbr != null && v.RegNbr.ToUpper().Contains(searchRegNbr));
            ViewData["Exists"] = exists;

            // If yes, filter. If not, show the full list.
            if (exists == true)
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.RegNbr != null && v.RegNbr.ToUpper().Contains(searchRegNbr));
            }
        }

        // 3. Package the vehicles in the ViewModel that your view is actually using (VehiclesViewModel)
        var model = new VehiclesViewModel // Creates an instance (container) to store and send vehicles back to view
        {
            ParkedVehicles = await vehiclesQuery.ToListAsync<ParkedVehicle>()
        };

        // 4. Returnera vyn
        return View(model);
    }



    // GET: PARKEDVEHICLES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PARKEDVEHICLES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ParkVehicleViewModel ParkVehicleViewModel)
    {
        if (ModelState.IsValid)
        {
            bool exists = _context.ParkedVehicle.Any(v => v.RegNbr == ParkVehicleViewModel.RegNbr);

            if (exists)
            {
                ModelState.AddModelError("RegNbr", "Registration number already exists in the garage. There can only be one vehicle per registration number.");
                return View(ParkVehicleViewModel);
            }

            var parkedVehicle = new ParkedVehicle
            {
                VehicleType = ParkVehicleViewModel.VehicleType,
                RegNbr = ParkVehicleViewModel.RegNbr?.ToUpper().Trim() ?? string.Empty,
                Color = ParkVehicleViewModel.Color ?? string.Empty,
                Brand = ParkVehicleViewModel.Brand ?? string.Empty,
                Model = ParkVehicleViewModel.Model ?? string.Empty,
                Wheels = ParkVehicleViewModel.Wheels,
                Arrival = DateTime.Now
            };
            _context.Add(parkedVehicle);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Vehicle has been successfully parked!";

            return RedirectToAction(nameof(Index));

        }

        TempData["Error"] = "Vehicle could not be parked!";

        return View(ParkVehicleViewModel);
    }

    // GET: PARKEDVEHICLES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "A valid vehicle ID must be provided to edit.";
            return RedirectToAction(nameof(Index));
        }

        var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);

        if (parkedvehicle == null)
        {
            TempData["ErrorMessage"] = $"Vehicle with ID {id} could not be found in the system.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new ParkedVehicleEditViewModel
        {
            Id = parkedvehicle.Id,
            VehicleType = parkedvehicle.VehicleType,
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
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Is there another vehicle that already has this reg number?
                bool regNbrExists = await _context.ParkedVehicle
                    .AnyAsync(v => v.RegNbr == viewModel.RegNbr && v.Id != viewModel.Id);

                if (regNbrExists)
                {
                    ModelState.AddModelError("RegNbr", "This registration number is already occupied by another parked vehicle.");

                    return View(viewModel); // "Abort" and send user back to view with error message
                }

                var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);

                // If the vehicle has been removed from the database while another user edited it
                if (parkedvehicle == null)
                {
                    TempData["ErrorMessage"] = "The vehicle you are trying to edit no longer exists.";
                    return RedirectToAction(nameof(Index));
                }

                parkedvehicle.VehicleType = viewModel.VehicleType!.Value;
                parkedvehicle.RegNbr = viewModel.RegNbr;
                parkedvehicle.Color = viewModel.Color;
                parkedvehicle.Brand = viewModel.Brand;
                parkedvehicle.Model = viewModel.Model;
                parkedvehicle.Wheels = viewModel.Wheels;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Vehicle with registration number \"{parkedvehicle.RegNbr}\" has been updated!";
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

            TempData["Success"] = "Vehicle details updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = "Failed to update vehicle details.";

        return View(viewModel);
    }


    // GET: PARKEDVEHICLES/Delete/5
    public async Task<IActionResult> Checkout(int? id)
    {
        ParkedVehicle? parkedVehicle = null;

        if (id == null ||
            (parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id)) == null)
        {
            return NotFound();
        }

        ReceiptViewModel receiptViewModel = CreateReceiptViewModel(parkedVehicle);

        return View(receiptViewModel);
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
                // Error...
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
        ParkedVehicle? parkedvehicle = await _context.ParkedVehicle.FindAsync(id);
        if (parkedvehicle == null)
        {
            return NotFound();
        }

        //Store data for receipt in TempData
        ReceiptViewModel receiptViewModel = CreateReceiptViewModel(parkedvehicle);
        TempData["receipt"] = JsonSerializer.Serialize(receiptViewModel);

        //Remove vehicle
        _context.ParkedVehicle.Remove(parkedvehicle);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Vehicle has been checked out.";

        return RedirectToAction("Receipt", "ParkedVehicles");
    }

    private bool ParkedVehicleExists(int? id)
    {
        return _context.ParkedVehicle.Any(e => e.Id == id);
    }

    private ReceiptViewModel CreateReceiptViewModel(ParkedVehicle parkedVehicle)
    {
        //Get act time, parked time and price
        var timeNow = DateTime.Now;
        TimeSpan totalTime = timeNow - parkedVehicle.Arrival;
        var timeDays = totalTime.Days;
        var timeHours = totalTime.Hours;
        var timeMinutes = totalTime.Minutes;

        //Calculate price
        var price = (timeDays * 24 * pricePerHour) + (timeHours * pricePerHour) + (timeMinutes * pricePerHour / 60);

        ReceiptViewModel receiptViewModel = new ReceiptViewModel()
        {
            Id = parkedVehicle.Id,
            VehicleType = parkedVehicle.VehicleType,
            RegNbr = parkedVehicle.RegNbr,
            Color = parkedVehicle.Color,
            Brand = parkedVehicle.Brand,
            Model = parkedVehicle.Model,
            Wheels = parkedVehicle.Wheels,
            Arrival = parkedVehicle.Arrival,
            CheckoutTime = timeNow,
            ParkedDays = timeDays,
            ParkedHours = timeHours,
            ParkedMinutes = timeMinutes,
            Price = price,
            PricePerHour = pricePerHour
        };

        return receiptViewModel;
    }
}
