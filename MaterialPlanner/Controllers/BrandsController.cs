using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaterialPlanner.Models;

namespace MaterialPlanner.Controllers
{
    public class BrandsController : Controller
    {
        private readonly MaterialPlannerContext _context;

        public BrandsController(MaterialPlannerContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var brands = _context.Brands.AsQueryable();

            if (startDate.HasValue)
            {
                brands = brands.Where(b => b.CreatedAt.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                brands = brands.Where(b => b.CreatedAt.Date <= endDate.Value.Date);
            }

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(await brands.ToListAsync());
        }





        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _context.Brands
                .FirstOrDefaultAsync(x => x.Id == id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brands brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brands brand)
        {
            if (id != brand.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Brands.Any(e => e.Id == brand.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _context.Brands
                .FirstOrDefaultAsync(x => x.Id == id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}