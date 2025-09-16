using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Repository;

namespace TechnicalShop.Repository.Components
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly DBDataContext _context;
        public ProductsViewComponent(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = await _context.Products.Include("Brand").Include("Category").ToListAsync();

            return View(product);
        } 
    }
}