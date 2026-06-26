using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaterialPlanner.Models;

namespace MaterialPlanner.Controllers
{
    public class UnitsController : Controller
    {
        private readonly MaterialPlannerContext _context;

        public UnitsController(MaterialPlannerContext context)
        {
            _context = context;
        }

        // GET: Units
        public async Task<IActionResult> Index(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var units = await _context.Units
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View(units);
        }

        // POST: Units/CreateAjax
        [HttpPost]
        public async Task<IActionResult> CreateAjax(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");

            name = name.Trim();

            // Evitar duplicados
            var existing = await _context.Units
                .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            if (existing != null)
            {
                return Ok(new
                {
                    id = existing.Id,
                    name = existing.Name
                });
            }

            var unit = new Units
            {
                Name = name,
                CreatedAt = DateTime.Now
            };

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = unit.Id,
                name = unit.Name
            });
        }

        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var unit = await _context.Units
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units.FindAsync(id);

            if (unit == null)
                return RedirectToAction(nameof(Index));

            try
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Unit deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Could not delete unit: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}