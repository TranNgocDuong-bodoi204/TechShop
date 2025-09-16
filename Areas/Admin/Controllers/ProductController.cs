using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechnicalShop.Repository;
using TechnicalShop.Models;
using TechnicalShop.Areas.Admin.Model;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly DBDataContext _context;

        public ProductController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            foreach (var product in products)
            {
                product.Category = await _context.Categories.FindAsync(product.CategoryId);
                product.Brand = await _context.Brands.FindAsync(product.BrandId);
            }

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var brand = await _context.Brands.ToListAsync();
            var category = await _context.Categories.ToListAsync();

            ViewBag.Categories = category;
            ViewBag.Brands = brand;

            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel product, IFormFile image)
        {
            
                var pr = _context.Products.FirstOrDefault(x => x.Name == product.Name);
                if(pr != null)
                {
                    string error = "Tên sản phẩm đã tồn tại";
                    TempData["error"] = error;
                    ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    ViewBag.Brands = await _context.Brands.ToListAsync();
                    return View(product);
                }
                ProductModel producModel = new ProductModel();
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    producModel.Name = product.Name;
                    producModel.Slug = product.Slug;
                    producModel.Description = product.Description;
                    producModel.Price = product.Price;
                    producModel.BrandId = product.BrandId;
                    producModel.CategoryId = product.CategoryId;
                    producModel.Image = fileName;
                    producModel.status = product.Status;
                    

                    await _context.Products.AddAsync(producModel);
                    await _context.SaveChangesAsync();
                    

                    InventoryModel inventory = new InventoryModel
                    {
                        ProductId = producModel.Id,
                        Quantity = product.Quantity,
                        LastUpdated = DateTime.Now
                    };

                    await _context.Inventories.AddAsync(inventory);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Index");
                

             }
            
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductModel product, IFormFile image)
        {
            var productInDb = await _context.Products.FindAsync(product.Id);
            if (productInDb == null)
            {
                return NotFound();
            }

            // Cập nhật ảnh nếu có
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                productInDb.Image = fileName;
            }

            // Cập nhật các field khác từ form
            productInDb.Name = product.Name;
            productInDb.Price = product.Price;
            productInDb.Description = product.Description;
            productInDb.BrandId = product.BrandId;
            productInDb.CategoryId = product.CategoryId;
            productInDb.Slug = product.Slug;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}