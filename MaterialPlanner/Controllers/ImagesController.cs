using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaterialPlanner.Models;

namespace MaterialPlanner.Controllers
{
    public class ImagesController : Controller
    {
        private readonly MaterialPlannerContext _context;

        public ImagesController(MaterialPlannerContext context)
        {
            _context = context;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var images = _context.Images
                .Include(i => i.MaterialDetails);

            return View(await images.ToListAsync());
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images
                .Include(i => i.MaterialDetails)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image == null) return NotFound();

            return View(image);
        }

        // GET: Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Create (FIXED UPLOAD + MODELSTATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Image image, IFormFile file)
        {
            // 🔥 limpiar validaciones problemáticas del modelo
            ModelState.Remove("MaterialDetails");
            ModelState.Remove("Path");
            ModelState.Remove("Description");

            if (string.IsNullOrWhiteSpace(image.Description))
            {
                image.Description = "Sin descripción";
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Debes seleccionar una imagen");
            }

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {item.Key} - Error: {error.ErrorMessage}");
                    }
                }

                LoadDropdowns();
                return View(image);
            }

            // carpeta destino
            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // nombre único
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // guardar ruta en BD
            image.Path = "/images/" + fileName;

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "MaterialDetails",
                new { id = image.MaterialDetailsId }
            );
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.FindAsync(id);

            if (image == null) return NotFound();

            LoadDropdowns();
            return View(image);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Image image)
        {
            if (id != image.Id) return NotFound();

            ModelState.Remove("MaterialDetails");
            ModelState.Remove("Path");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(image);
            }

            _context.Update(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images
                .Include(i => i.MaterialDetails)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image == null) return NotFound();

            return View(image);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image != null)
            {
                var materialId = image.MaterialDetailsId;

                // borrar archivo físico si existe
                var fullPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    image.Path.TrimStart('/')
                );

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                _context.Images.Remove(image);
                await _context.SaveChangesAsync();

                // 🔥 REGRESAR AL MATERIAL
                return RedirectToAction("Details", "MaterialDetails", new { id = materialId });
            }

            return RedirectToAction(nameof(Index));
        }

        // Dropdowns
        private void LoadDropdowns()
        {
            ViewBag.MaterialDetailsId = new SelectList(
                _context.MaterialDetails
                    .Include(m => m.Material)
                    .Include(m => m.Brand)
                    .Include(m => m.Product),
                "Id",
                "Id"
            );
        }
    }
}