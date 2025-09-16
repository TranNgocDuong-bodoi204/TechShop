using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models.DTO
{
    public class AccountViewDTO
    {
        public string Username;
        public string Email;
        public string PhoneNumber;
        public string Address;
        public DateTime CreatedAt  = DateTime.Now;
        public string Role = "User";
    }
}