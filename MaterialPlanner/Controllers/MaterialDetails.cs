using ClosedXML.Excel;
using MaterialPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using X.PagedList;
using X.PagedList.Extensions;


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
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? page)
        {
            var materialDetails = _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation)
                .Include(m => m.Images)
                .Include(m => m.Unit)
                .AsQueryable();

          
            // filtro por fecha
            
            if (startDate.HasValue)
            {
                materialDetails = materialDetails.Where(m => m.CreatedAt.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                materialDetails = materialDetails.Where(m => m.CreatedAt.Date <= endDate.Value.Date);
            }

            // Se conservan las fechas para que la vista no pierda los filtros
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

           
            //Paginacion
            
            //cantidad de registros que se mostrarán por pagina
           
            int pageSize = 10;

            
            //se mostrará la primera.
            int pageNumber = page ?? 1;

            
            //Ordenado de datos
            
            materialDetails = materialDetails
                .OrderByDescending(m => m.CreatedAt);

            //devuelve los datos paginados a la vista
            return View(materialDetails.ToPagedList(pageNumber, pageSize));
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

        public async Task<IActionResult> ExportToExcel(DateTime? startDate, DateTime? endDate)
        {
            var materialDetailsQuery = _context.MaterialDetails
                .Include(m => m.Material)
                .Include(m => m.Brand)
                .Include(m => m.Product)
                .Include(m => m.Presentation)
                .Include(m => m.Unit)
                .AsQueryable();

            if (startDate.HasValue)
            {
                materialDetailsQuery = materialDetailsQuery
                    .Where(m => m.CreatedAt.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                materialDetailsQuery = materialDetailsQuery
                    .Where(m => m.CreatedAt.Date <= endDate.Value.Date);
            }

            var materialDetails = await materialDetailsQuery.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Material Details");

            worksheet.Cell(1, 1).Value = "Material";
            worksheet.Cell(1, 2).Value = "Item Material";
            worksheet.Cell(1, 3).Value = "Brand";
            worksheet.Cell(1, 4).Value = "Product";
            worksheet.Cell(1, 5).Value = "SKU";
            worksheet.Cell(1, 6).Value = "Presentation";
            worksheet.Cell(1, 7).Value = "Status";
            worksheet.Cell(1, 8).Value = "Consumption";
            worksheet.Cell(1, 9).Value = "Unit";
            worksheet.Cell(1, 10).Value = "Created At";

            int row = 2;

            foreach (var item in materialDetails)
            {
                worksheet.Cell(row, 1).Value = item.Material?.Description;
                worksheet.Cell(row, 2).Value = item.Material?.ItemMaterial;
                worksheet.Cell(row, 3).Value = item.Brand?.Name;
                worksheet.Cell(row, 4).Value = item.Product?.Description;
                worksheet.Cell(row, 5).Value = item.Product?.SKU;
                worksheet.Cell(row, 6).Value = item.Presentation?.Name;
                worksheet.Cell(row, 7).Value = item.Status ? "Active" : "Inactive";
                worksheet.Cell(row, 8).Value = item.Consumption;
                worksheet.Cell(row, 9).Value = item.Unit?.Name;
                worksheet.Cell(row, 10).Value = item.CreatedAt.ToString("dd/MM/yyyy hh:mm tt");

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "MaterialDetails.xlsx"
            );
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