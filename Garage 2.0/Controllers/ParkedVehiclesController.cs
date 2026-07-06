
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;

public class ParkedVehiclesController : Controller
{
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
    public async Task<IActionResult> Create([Bind("Id,VehicleType,RegNbr,Color,Brand,Model,Wheels,Arrival")] ParkedVehicle parkedvehicle)
    {
        if (ModelState.IsValid)
        {
            _context.Add(parkedvehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(parkedvehicle);
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
        return View(parkedvehicle);
    }

    // POST: PARKEDVEHICLES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,VehicleType,RegNbr,Color,Brand,Model,Wheels,Arrival")] ParkedVehicle parkedvehicle)
    {
        if (id != parkedvehicle.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // KONTROLL: Finns det ett ANNAT fordon som redan har detta regnummer?
                bool regNbrExists = await _context.ParkedVehicle
                    .AnyAsync(v => v.RegNbr == parkedvehicle.RegNbr && v.Id != parkedvehicle.Id);

                if (regNbrExists)
                {
                    // Lägg till ett valideringsfel kopplat till just fältet RegNbr
                    ModelState.AddModelError("RegNbr", "Registreringsnumret är upptaget av ett annat parkerat fordon.");

                    // Avbryt och skicka tillbaka användaren till vyn med felmeddelandet visat
                    return View(parkedvehicle);
                }

                _context.Update(parkedvehicle);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Vehicle with registration number \"{parkedvehicle.RegNbr}\" has been updated!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(parkedvehicle.Id))
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
        return View(parkedvehicle);
    }


    // GET: PARKEDVEHICLES/Delete/5
    public async Task<IActionResult> Delete(int? id)
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
