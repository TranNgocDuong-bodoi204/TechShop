
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{
    [Route("[controller]")]
    public class Brands : Controller
    {
        private readonly ILogger<Brands> _logger;
        private readonly DBDataContext _context;

        public Brands(ILogger<Brands> logger, DBDataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string slug = "")
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Slug == slug);

            if (brand == null) return RedirectToAction("Index");

            ViewBag.Name = brand.Name;

            var products = await _context.Products.Where(x => x.BrandId == brand.Id).ToListAsync();


            return View(products);
        }
    }
}