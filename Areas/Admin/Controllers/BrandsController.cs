using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Repository;
using TechnicalShop.Models;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {

        private readonly DBDataContext _context;

        public BrandsController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _context.Brands.ToArrayAsync();

            if (brands == null) return NotFound();
            return View(brands);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandModel brandModel)
        {
            var brand = new BrandModel
            {
                Name = brandModel.Name,
                Description = brandModel.Description,
                Slug = brandModel.Slug,
                Status = brandModel.Status
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(brand);
                await _context.SaveChangesAsync();
            }

            return View("Index", await _context.Brands.ToListAsync());
        }
        public async Task<IActionResult> Update(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand != null)
            {
                return View(brand);
            }
            return View("Index", await _context.Brands.ToListAsync());
        }    
        [HttpPost]
        public async Task<IActionResult> Update(int id, BrandModel brandModel)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return View("Update", brandModel);
            }

            brand.Name = brandModel.Name;
            brand.Description = brandModel.Description;
            brand.Slug = brandModel.Slug;
            brand.Status = brandModel.Status;

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();

            return View("index", await _context.Brands.ToListAsync());
        }

        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return View("Index", _context.Brands.ToList());
        }
    }
}