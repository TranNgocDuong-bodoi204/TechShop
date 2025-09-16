using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc; // Remove this duplicate if already present above
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Models;
using TechnicalShop.Repository;
using Microsoft.AspNetCore.Mvc;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Microsoft.AspNetCore.Mvc.Controller
    {

        private readonly DBDataContext _context;

        public CategoryController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CategoryModel category)
        {
            var cate = new CategoryModel
            {
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
                Status = category.Status
            };

            if (ModelState.IsValid)
            {
                _context.Categories.Add(cate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public async Task<IActionResult> Update(int id)
        {
            var cate = await _context.Categories.FindAsync(id);

            if (cate == null)
            {
                return NotFound();
            }

            return View(cate);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryModel category, int id)
        {
            var cate = await _context.Categories.FindAsync(id);
            if (cate == null)
            {
                return NotFound();
            }
            cate.Name = category.Name;
            cate.Description = category.Description;
            cate.Slug = category.Slug;
            cate.Status = category.Status;
            if (ModelState.IsValid)
            {
                _context.Categories.Update(cate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Index", await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cate = await _context.Categories.FindAsync(id);
            if (cate == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(cate);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}