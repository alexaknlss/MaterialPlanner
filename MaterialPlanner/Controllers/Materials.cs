using MaterialPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace MaterialPlanner.Controllers
{
    [Authorize]
    public class MaterialsController : Controller
    {
        private readonly MaterialPlannerContext _context;

        public MaterialsController(MaterialPlannerContext context)
        {
            _context = context;
        }

        // index: Materials
       
        public async Task<IActionResult> Index(int? page)
        {
            var materials = _context.Materials.AsQueryable();

           
            // Orden
          
            materials = materials.OrderByDescending(m => m.Id);

            
            //Pagination
           
            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(materials.ToPagedList(pageNumber, pageSize));
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null) return NotFound();

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materials material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var material = await _context.Materials.FindAsync(id);
            if (material == null) return NotFound();

            return View(material);
        }

        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Materials material)
        {
            if (id != material.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Materials.Any(e => e.Id == material.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null) return NotFound();

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}