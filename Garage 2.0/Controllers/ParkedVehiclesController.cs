
using Garage_2._0.Models;
using Garage_2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

public class ParkedVehiclesController : Controller
{
    const int pricePerHour = 10;

    private readonly Garage_2_0Context _context;

    public ParkedVehiclesController(Garage_2_0Context context)
    {
        _context = context;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ParkedVehicle.ToListAsync());
    }

    // GET: PARKEDVEHICLES/Search (IndexWithViewModel)
    public async Task<IActionResult> Search(string searchRegNbr)
    {
        // Utgångspunkten är alla fordon i databasen
        var vehiclesQuery = _context.ParkedVehicle.AsQueryable();
        bool? exists = null;

            // Om användaren faktiskt har skrivit något i sökfältet
            if (!string.IsNullOrEmpty(searchRegNbr))
            {
                searchRegNbr = searchRegNbr.Trim().ToUpper();
                ViewData["CurrentFilter"] = searchRegNbr; // Sparar texten i sökfältet

                // Kontrollera om det exakta numret finns
                exists = _context.ParkedVehicle.Any(v => v.RegNbr != null && v.RegNbr.ToUpper().Contains(searchRegNbr));
                ViewData["Exists"] = exists;

                    // Om det finns, filtrera. Om inte, visas hela listan
                    if (exists == true)
                    {
                        vehiclesQuery = vehiclesQuery.Where(v => v.RegNbr != null && v.RegNbr.ToUpper().Contains(searchRegNbr));
                    }
            }

            // 3. Packa in bilarna i den ViewModel som din vy faktiskt använder (VehiclesViewModel)
            var model = new VehiclesViewModel // Skapar en instans (en behållare) för att förvara och skicka bilarna till vyn
                {
                    ParkedVehicles = await vehiclesQuery.ToListAsync<ParkedVehicle>()
                };

        // 4. Returnera vyn
        return View(model);
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
    public async Task<IActionResult> Create(ParkedVehicleViewModel parkedVehicleViewModel)
    {
        if (ModelState.IsValid)
        {
            var parkedVehicle = new ParkedVehicle
            {
                VehicleType = parkedVehicleViewModel.VehicleType,
                RegNbr = parkedVehicleViewModel.RegNbr,
                Color = parkedVehicleViewModel.Color,
                Brand = parkedVehicleViewModel.Brand,
                Model = parkedVehicleViewModel.Model,
                Wheels = parkedVehicleViewModel.Wheels,
                Arrival = DateTime.Now
            };
            //_context.Add(parkedVehicleViewModel);
            _context.Add(parkedVehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        return View(parkedVehicleViewModel);
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
        // 1. Om ID i URL:en inte matchar ID i formuläret (Manipulerad begäran)
        if (id != viewModel.Id)
        {
            TempData["ErrorMessage"] = "Data mismatch error. The request could not be processed.";
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            try
            {
                // KONTROLL: Finns det ett ANNAT fordon som redan har detta regnummer?
                bool regNbrExists = await _context.ParkedVehicle
                    .AnyAsync(v => v.RegNbr == viewModel.RegNbr && v.Id != viewModel.Id);

                if (regNbrExists)
                {
                    ModelState.AddModelError("RegNbr", "This registration number is already occupied by another parked vehicle.");

                    return View(viewModel); // Avbryt och skicka tillbaka användaren till vyn med felmeddelandet visat
                }

                var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);

                // 2. Om fordonet har tagits bort från databasen under tiden användaren redigerade det
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
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }


    // GET: PARKEDVEHICLES/Delete/5
    public async Task<IActionResult> Checkout(int? id)
    {
        ParkedVehicle? parkedVehicle = null;

        //Bad input or vehicle not found
        if (id == null ||
            (parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id)) == null)
        {
            return NotFound();
        }

        //Get act time, parked time and price
        var timeNow = DateTime.Now;
        TimeSpan totalTime = timeNow - parkedVehicle.Arrival;
        var timeHours = totalTime.Hours;
        var timeMinutes = totalTime.Minutes;

        //Create parked time as string
        StringBuilder strTime = new StringBuilder();
        if (timeHours > 0)
            strTime.Append(timeHours + " h ");
        if (timeMinutes > 0)
            strTime.Append(timeMinutes + " m");
        
        //Collect data to View
        ViewBag.timeNow = timeNow;
        ViewBag.totalTimeString = strTime;
        ViewBag.price = (timeHours * pricePerHour) + (timeMinutes * pricePerHour / 60);

        return View(parkedVehicle);
    }

    // POST: PARKEDVEHICLES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);
        if (parkedvehicle != null)
        {
            _context.ParkedVehicle.Remove(parkedvehicle);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ParkedVehicleExists(int? id)
    {
        return _context.ParkedVehicle.Any(e => e.Id == id);
    }
}
