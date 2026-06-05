using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaterialPlanner.Models;

namespace MaterialPlanner.Controllers
{
    public class PresentationsController : Controller
    {
        private readonly MaterialPlannerContext _context;

        public PresentationsController(MaterialPlannerContext context)
        {
            _context = context;
        }

        // GET: Presentations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Presentation.ToListAsync());
        }

        // GET: Presentations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var presentation = await _context.Presentation
                .FirstOrDefaultAsync(m => m.Id == id);

            if (presentation == null) return NotFound();

            return View(presentation);
        }

        // GET: Presentations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Presentations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Presentation presentation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(presentation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(presentation);
        }

        // GET: Presentations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var presentation = await _context.Presentation.FindAsync(id);
            if (presentation == null) return NotFound();

            return View(presentation);
        }

        // POST: Presentations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Presentation presentation)
        {
            if (id != presentation.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presentation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Presentation.Any(e => e.Id == presentation.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(presentation);
        }

        // GET: Presentations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var presentation = await _context.Presentation
                .FirstOrDefaultAsync(m => m.Id == id);

            if (presentation == null) return NotFound();

            return View(presentation);
        }

        // POST: Presentations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presentation = await _context.Presentation.FindAsync(id);
            _context.Presentation.Remove(presentation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}