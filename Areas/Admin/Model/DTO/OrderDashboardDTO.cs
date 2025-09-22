using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;

namespace TechnicalShop.Areas.Admin.Model.DTO
{
    public class OrderDashboardDTO
    {
        public int total { get; set; }
        public int notaccepted { get; set; }
        public int accepted { get; set; }
        public int shipping { get; set; }
        public int completed { get; set; }
        public int canceled { get; set; }
        public List<Order> orderNotAccept { get; set; }
        public List<Order> orderAccepted { get; set; }

        public OrderDashboardDTO()
        {
            this.total = 0;
            this.notaccepted = 0;
            this.accepted = 0;
            this.shipping = 0;
            this.completed = 0;
            this.canceled = 0;
            orderNotAccept = new List<Order>();
            orderAccepted = new List<Order>();
        }
    }
}