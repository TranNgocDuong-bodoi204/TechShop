using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Models.DTO;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{
    public class SearchController : Controller
    {
        private readonly DBDataContext _context;

        public SearchController(DBDataContext context)
        {
            this._context = context;
        }
        public IActionResult Index(string keyword)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword));
            }

            var products = query
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToList();

            return View(products);
        }

        [HttpGet]
public async Task<IActionResult> Filter(int? CategoryId, int? BrandId, decimal? MinPrice, decimal? MaxPrice, string status)
{
    // Lấy danh sách gốc
    var query = _context.Products.AsQueryable();

    // Lọc theo danh mục
    if (CategoryId.HasValue && CategoryId > 0)
    {
        query = query.Where(p => p.CategoryId == CategoryId.Value);
    }

    // Lọc theo thương hiệu
    if (BrandId.HasValue && BrandId > 0)
    {
        query = query.Where(p => p.BrandId == BrandId.Value);
    }

    // Lọc theo giá
    if (MinPrice.HasValue)
    {
        query = query.Where(p => p.Price >= MinPrice.Value);
    }
    if (MaxPrice.HasValue)
    {
        query = query.Where(p => p.Price <= MaxPrice.Value);
    }

    // Lọc theo trạng thái
    if (!string.IsNullOrEmpty(status))
    {
        if (status == "in-stock")
        {
            query = query.Where(p => p.status == "InStock");
        }
        else if (status == "out-of-stock")
        {
            query = query.Where(p => p.status == "OutStock");
        }
    }

    // Truy vấn dữ liệu
    var products = await query.ToListAsync();
    var brands = await _context.Brands.ToListAsync();
    var categories = await _context.Categories.ToListAsync();

    // Đóng gói vào DTO
    var data = new HomeViewDTO
    {
        Products = products,
        Brands = brands,
        Categories = categories
    };

    return View(data); 
}


    }
    
}