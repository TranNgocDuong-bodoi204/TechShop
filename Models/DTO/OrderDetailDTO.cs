using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models.DTO;
using TechnicalShop.Models.Enum;

namespace TechnicalShop.Models.DTO
{
    public class OrderDetailDTO
    {
        public int id { get; set; }
        public DateTime orderAt { get; set; }
        public OrderStatus orderStatus { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public List<OrderItemDetail> orderDetails { get; set; }
        public OrderDetailDTO(){}

        public void SetOrderDetailInfo(Order order, List<OrderItemDetail> orderDetails)
        {
            this.id = order.OrderId;
            this.orderAt = order.CreatedDate;
            this.orderStatus = order.orderStatus;
            this.paymentMethod = order.PaymentMethod;
            this.paymentStatus = order.paymentStatus;
            this.TotalAmount = order.TotalAmount;
            this.CustomerName = order.CustomerName;
            this.Email = order.Email;
            this.Phone = order.Phone;
            this.Address = order.Address;
            this.orderDetails = orderDetails;
        }
    }
}