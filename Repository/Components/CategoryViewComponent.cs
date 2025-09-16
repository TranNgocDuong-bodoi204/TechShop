using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Repository;

namespace TechnicalShop.Repository.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly DBDataContext _context;
        public CategoryViewComponent(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _context.Categories.ToListAsync());
    }
}