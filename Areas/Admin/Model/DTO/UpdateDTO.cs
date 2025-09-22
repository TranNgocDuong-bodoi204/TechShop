using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;
using TechnicalShop.Models.Enum;

namespace TechnicalShop.Areas.Admin.Model.DTO
{
    public class UpdateDTO
    {
        public int id { get; set; }
        public DateTime orderedAt { get; set; }
        public string customerName { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public decimal totalPrice { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public PaymentStatus paymentStatus{ get; set; }
        public OrderStatus orderStatus { get; set; }

        public UpdateDTO(){}

        public UpdateDTO(Order order)
        {
            this.id = order.OrderId;
            this.orderedAt = order.CreatedDate;
            this.customerName = order.CustomerName;
            this.phone = order.Phone;
            this.Email = order.Email;
            this.totalPrice = order.TotalAmount;
            this.paymentStatus = order.paymentStatus;
            this.paymentMethod = order.PaymentMethod;
            this.orderStatus = order.orderStatus;
        }
    }
}