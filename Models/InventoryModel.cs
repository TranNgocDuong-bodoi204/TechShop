using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;

namespace TechnicalShop.Models
{
    public class InventoryModel
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}