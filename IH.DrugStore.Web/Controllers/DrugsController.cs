using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IH.DrugStore.Web.Data;
using IH.DrugStore.Web.Data.Entities;
using AutoMapper;
using IH.DrugStore.Web.Models.Drugs;

namespace IH.DrugStore.Web.Controllers
{
    public class DrugsController : Controller
    {
        #region Data and Const

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DrugsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var drugs = await _context
                                    .Drugs
                                    .Include(drug => drug.DrugType)
                                    .ToListAsync();

            var drugVMs = _mapper.Map<List<Drug>, List<DrugListViewModel>>(drugs);

            return View(drugVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context
                                .Drugs
                                .Include(drug => drug.DrugType)
                                .Where(drug => drug.Id == id) // Id from URL example: 17
                                .SingleOrDefaultAsync();

            if (drug == null)
            {
                return NotFound();
            }

            var drugVM = _mapper.Map<Drug, DrugDetailsViewModel>(drug);

            return View(drugVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUpdateDrugViewModel drugVM)
        {
            if (ModelState.IsValid)
            {
                var drug = _mapper.Map<CreateUpdateDrugViewModel, Drug>(drugVM);

                _context.Add(drug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(drugVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context.Drugs.FindAsync(id);
            if (drug == null)
            {
                return NotFound();
            }
            return View(drug);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Drug drug)
        {
            if (id != drug.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrugExists(drug.Id))
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
            return View(drug);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context.Drugs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drug = await _context.Drugs.FindAsync(id);
            if (drug != null)
            {
                _context.Drugs.Remove(drug);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Private Methods

        private bool DrugExists(int id)
        {
            return _context.Drugs.Any(e => e.Id == id);
        }

        #endregion
    }
}
