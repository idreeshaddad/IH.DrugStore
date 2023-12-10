using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IH.DrugStore.Web.Data;
using IH.DrugStore.Web.Data.Entities;

namespace IH.DrugStore.Web.Controllers
{
    public class DrugTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrugTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DrugTypes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drugType = await _context.DrugTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drugType == null)
            {
                return NotFound();
            }

            return View(drugType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DrugType drugType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drugType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drugType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drugType = await _context.DrugTypes.FindAsync(id);
            if (drugType == null)
            {
                return NotFound();
            }
            return View(drugType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DrugType drugType)
        {
            if (id != drugType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drugType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrugTypeExists(drugType.Id))
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
            return View(drugType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drugType = await _context.DrugTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drugType == null)
            {
                return NotFound();
            }

            return View(drugType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drugType = await _context.DrugTypes.FindAsync(id);
            if (drugType != null)
            {
                _context.DrugTypes.Remove(drugType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrugTypeExists(int id)
        {
            return _context.DrugTypes.Any(e => e.Id == id);
        }
    }
}
