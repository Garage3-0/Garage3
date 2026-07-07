
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;
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
            return NotFound();
        }

        var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);
        if (parkedvehicle == null)
        {
            return NotFound();
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
        if (id != viewModel.Id)
        {
            return NotFound();
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
                    // Lägg till ett valideringsfel kopplat till just fältet RegNbr
                    ModelState.AddModelError("RegNbr", "This registration number is already occupied by another parked vehicle.");

                    // Avbryt och skicka tillbaka användaren till vyn med felmeddelandet visat
                    return View(viewModel);
                }

                var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);
                if (parkedvehicle == null)
                {
                    return NotFound();
                }

                parkedvehicle.VehicleType = viewModel.VehicleType!.Value;
                parkedvehicle.RegNbr = viewModel.RegNbr;
                parkedvehicle.Color = viewModel.Color;
                parkedvehicle.Brand = viewModel.Brand;
                parkedvehicle.Model = viewModel.Model;
                parkedvehicle.Wheels = viewModel.Wheels;

                //_context.Update(parkedvehicle);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Vehicle with registration number \"{parkedvehicle.RegNbr}\" has been updated!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
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
