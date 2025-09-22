using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;

namespace TechnicalShop.Models
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get => (decimal)(Quantity * Price); }
        public string Image { get; set; }
        public CartItemModel() { }

        public CartItemModel(ProductModel product)
        {
            this.ProductId = product.Id;
            this.ProductName = product.Name;
            this.Quantity = 1;
            this.Price = product.Price;
            this.Image = product.Image;
        }
    }
}