using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models.Enum;

namespace TechnicalShop.Models.DTO
{
    public class OrderViewDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderAt { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus orderstatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus paymentStatus { get; set; } = PaymentStatus.Unpaid;
    }
}