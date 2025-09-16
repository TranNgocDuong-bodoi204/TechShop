using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Models;
using TechnicalShop.Models.ViewModels;
using TechnicalShop.Repository;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoryController : Controller
    {
        private readonly DBDataContext _context;

        public InventoryController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var listInventory = await _context.Inventories.ToListAsync();

            InventoryViewModel inventoryView = new InventoryViewModel
            {
                Inventories = listInventory,
                totalProducts = listInventory.Count,

                numberOfProductsLowSock = listInventory.Count(x => x.Quantity < 10 && x.Quantity > 0),
                numberOfProductsOutOfStck = listInventory.Count(x => x.Quantity == 0)
            };

            List<string> productNames = new List<string>();
            foreach (var inventory in listInventory)
            {
                var product = await _context.Products.FindAsync(inventory.ProductId);
                if (product != null)
                {
                    productNames.Add(product.Name);
                }
            }

            inventoryView.productNames = productNames;



            return View(inventoryView);
        }
        public async Task<IActionResult> Update(int id)
        {
            var inventory = _context.Inventories.Find(id);
            if(inventory == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == inventory.ProductId);
            ViewBag.ProductName = product?.Name;
            return View(inventory);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, InventoryModel inventoryModel)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            inventory.Quantity = inventoryModel.Quantity;
            inventory.LastUpdated = DateTime.Now;

            var prd = await _context.Products.FindAsync(inventory.ProductId);
            if (inventory.Quantity == 0)
                prd.status = "OutStock";
            else
            {
                prd.status = "InStock";
            }

            await _context.SaveChangesAsync();

            var listInventory = await _context.Inventories.ToListAsync();

            InventoryViewModel inventoryView = new InventoryViewModel
            {
                Inventories = listInventory,
                totalProducts = listInventory.Count,

                numberOfProductsLowSock = listInventory.Count(x => x.Quantity < 10 && x.Quantity > 0),
                numberOfProductsOutOfStck = listInventory.Count(x => x.Quantity == 0)
            };

            List<string> productNames = new List<string>();
            foreach (var iv in listInventory)
            {
                var product = await _context.Products.FindAsync(inventory.ProductId);
                if (product != null)
                {
                    productNames.Add(product.Name);
                }
            }

            inventoryView.productNames = productNames;
            
            return View("Index", inventoryView);

        }

    }
}