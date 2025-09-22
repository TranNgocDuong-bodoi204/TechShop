using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models.DTO
{
    public class CheckoutViewDTO
    {
        public List<CartItemModel> products;
        public decimal TotalAmount;
        public int AccountId;
        public CheckoutViewDTO()
        {
            this.TotalAmount = 0;
        }
        public CheckoutViewDTO(List<CartItemModel> products, decimal TotalAmount, int AccountId)
        {
            this.products = products;
            this.TotalAmount = TotalAmount;
            this.AccountId = AccountId;
        }
    }
}