using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Areas.Admin.Model.DTOdata
{
    public class OrderUpdateJson
    {
        public string id { get; set; }
        public string paymentStatus { get; set; }
        public string orderStatus { get; set; }
    }
}