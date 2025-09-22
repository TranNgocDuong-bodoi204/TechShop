using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models.Enum;

namespace TechnicalShop.Models.DTO
{
    public class CheckoutSuccess
    {
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod paymentMethod { get; set; }
    }
}