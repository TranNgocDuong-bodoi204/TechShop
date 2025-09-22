using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models.Enum;

namespace TechnicalShop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int AccountId { get; set;}
        public AccountModel Account { get; set; }

        [Required, StringLength(100)]
        public string CustomerName { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, StringLength(20)]
        public string Phone { get; set; }
        public string Email { get; set; }

        [Required, StringLength(50)]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.COD;
        [Required]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus paymentStatus { get; set; } = PaymentStatus.Unpaid;
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}