using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models.Enum
{
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipping = 3,
        Completed = 4,
        Cancelled = 5
    }
}