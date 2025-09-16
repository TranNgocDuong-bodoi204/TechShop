using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Controllers.DTO;
using TechnicalShop.Repository;

namespace TechShopOnline.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DBDataContext _context;
        public CategoryController(DBDataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == Slug);


            if (category == null) return RedirectToAction("Index");

            ViewBag.Name = category.Name;
            
            var product = await _context.Products.Where(x => x.CategoryId == category.Id).ToListAsync();
            return View(product);
        }
    }
}
