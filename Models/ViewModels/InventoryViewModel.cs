using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models.ViewModels
{
    public class InventoryViewModel
    {
        public List<InventoryModel> Inventories { get; set; }
        public List<string> productNames { get; set; }
        public int totalProducts { get; set; }
        public int numberOfProductsLowSock { get; set; }
        public int numberOfProductsOutOfStck { get; set; }
    }
}  