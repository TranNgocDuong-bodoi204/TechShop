using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Models.DTO;
using TechnicalShop.Models;
using TechnicalShop.Repository;

namespace TechShopOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBDataContext _context;

        public HomeController(ILogger<HomeController> logger, DBDataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            var brands = await _context.Brands.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var data = new HomeViewDTO
            {
                Products = products,
                Brands = brands,
                Categories = categories
            };

            return View(data);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
