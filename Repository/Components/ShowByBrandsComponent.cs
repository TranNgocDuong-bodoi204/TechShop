using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Models;
using TechnicalShop.Repository;
using TechnicalShop.Models;

namespace TechnicalShop.Repository.Components
{
    public class ShowByBrandsComponent : ViewComponent
    {
        private readonly DBDataContext _context;
        public ShowByBrandsComponent(DBDataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            /*
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            var dics = products
                .GroupBy(p => p.Brand.Name)
                .ToDictionary(g => g.Key, g => g.ToList());
                */
            Dictionary<BrandModel, List<ProductModel>> dics = new Dictionary<BrandModel, List<ProductModel>>();

            var brands = await _context.Brands.ToListAsync();

            for (int i = 0; i < brands.Count; i++)
            {
                List<ProductModel> products = await _context.Products.Where(x => x.BrandId == brands[i].Id).ToListAsync();

                dics.Add(brands[i], products);
            }

            return View("Default", dics);
        }
    }
}