using MaterialPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;


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
                .Include(m => m.Presentation)
                .Include(m => m.Images)
                .Include(m => m.Unit); // 🆕 Units

            return View(await materialDetails.ToListAsync());
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var materialDetail = await _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation)
                .Include(m => m.Images)
                .Include(m => m.Unit) // 🆕 Units
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materialDetail == null)
                return NotFound();

            return View(materialDetail);
        }

        // GET: Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialDetails materialDetail)
        {
            ModelState.Remove("Material");
            ModelState.Remove("Brand");
            ModelState.Remove("Product");
            ModelState.Remove("Presentation");
            ModelState.Remove("Images");
            ModelState.Remove("Unit"); // 🆕 importante

            if (ModelState.IsValid)
            {
                _context.MaterialDetails.Add(materialDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(materialDetail);
        }

        // GET: Edit
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

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialDetails materialDetail)
        {
            if (id != materialDetail.Id)
                return NotFound();

            ModelState.Remove("Material");
            ModelState.Remove("Brand");
            ModelState.Remove("Product");
            ModelState.Remove("Presentation");
            ModelState.Remove("Images");
            ModelState.Remove("Unit"); // 🆕

            if (ModelState.IsValid)
            {
                var existing = await _context.MaterialDetails
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existing == null)
                    return NotFound();

                existing.MaterialId = materialDetail.MaterialId;
                existing.BrandId = materialDetail.BrandId;
                existing.ProductId = materialDetail.ProductId;
                existing.PresentationId = materialDetail.PresentationId;
                existing.UnitId = materialDetail.UnitId; // 🆕 IMPORTANTE
                existing.Status = materialDetail.Status;
                existing.Consumption = materialDetail.Consumption;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(materialDetail);
        }
        public async Task<IActionResult> ReportePDF(int id)
        {
            var material = await _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation)
                .Include(m => m.Unit)
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null)
                return NotFound();

            return new ViewAsPdf("Print", material)
            {
                FileName = $"Material_{id}.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                PageMargins = new Margins(10, 10, 10, 10)
            };
        }

        // DELETE (igual)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialDetail = await _context.MaterialDetails
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materialDetail != null)
            {
                _context.MaterialDetails.Remove(materialDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
       

        // DROPDOWNS
        private void LoadDropdowns()
        {
            ViewBag.MaterialId = new SelectList(_context.Materials, "Id", "Description");
            ViewBag.BrandId = new SelectList(_context.Brands, "Id", "Name");
            ViewBag.ProductId = new SelectList(_context.Products, "Id", "Description");
            ViewBag.PresentationId = new SelectList(_context.Presentations, "Id", "Name");

            // 🆕 UNITS
            ViewBag.Units = _context.Units
                .Select(u => new { u.Id, u.Name })
                .ToList();
        }
    }
}