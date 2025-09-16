using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Repository;

namespace TechnicalShop.Repository.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly DBDataContext _context;
        public BrandsViewComponent(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _context.Brands.ToListAsync());
    }
}