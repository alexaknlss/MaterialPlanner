using MaterialPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MaterialPlanner.Controllers
{
    public class MaterialDetailsController : Controller
    {
        private readonly MaterialPlannerContext _context;

        public MaterialDetailsController(MaterialPlannerContext context)
        {
            _context = context;
        }

        // GET: MaterialDetails
        public async Task<IActionResult> Index()
        {
            var materialDetails = _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation);

            return View(await materialDetails.ToListAsync());
        }

        // GET: MaterialDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var materialDetail = await _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materialDetail == null)
                return NotFound();

            return View(materialDetail);
        }

        // GET: MaterialDetails/Create
        public IActionResult Create()
        {
            LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialDetails materialDetail)
        {
            // remover validación de navegaciones
            ModelState.Remove("Material");
            ModelState.Remove("Brand");
            ModelState.Remove("Product");
            ModelState.Remove("Presentation");

            if (ModelState.IsValid)
            {
                _context.MaterialDetails.Add(materialDetail);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(materialDetail);
        }

        // GET: MaterialDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var materialDetail = await _context.MaterialDetails.FindAsync(id);

            if (materialDetail == null)
                return NotFound();

            LoadDropdowns();

            return View(materialDetail);
        }

        // POST: MaterialDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialDetails materialDetail)
        {
            if (id != materialDetail.Id)
                return NotFound();

            // 🔥 ignorar navegación para evitar ModelState inválido
            ModelState.Remove("Material");
            ModelState.Remove("Brand");
            ModelState.Remove("Product");
            ModelState.Remove("Presentation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MaterialDetails.Any(e => e.Id == materialDetail.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(materialDetail);
        }

        // GET: MaterialDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var materialDetail = await _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materialDetail == null)
                return NotFound();

            return View(materialDetail);
        }

        // POST: MaterialDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialDetail = await _context.MaterialDetails.FindAsync(id);

            if (materialDetail != null)
            {
                _context.MaterialDetails.Remove(materialDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewBag.MaterialId = new SelectList(
                _context.Materials,
                "Id",
                "Description");

            ViewBag.BrandId = new SelectList(
                _context.Brands,
                "Id",
                "Name");

            ViewBag.ProductId = new SelectList(
                _context.Products,
                "Id",
                "Description");

            ViewBag.PresentationId = new SelectList(
                _context.Presentation,
                "Id",
                "Name");
        }
    }
}