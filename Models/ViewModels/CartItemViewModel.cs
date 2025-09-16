using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> listCartItem { get; set; }
        public decimal GrandTotal { get; set; }
    }
}