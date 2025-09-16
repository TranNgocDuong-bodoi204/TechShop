using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;
using TechnicalShop.Models.DTO;

namespace TechnicalShop.Mapper
{
    public class AccountToDTO
    {
        public AccountViewDTO ToDTO(AccountModel model)
        {
            AccountViewDTO dto = new AccountViewDTO
            {
                Username = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CreatedAt = model.CreatedAt,
                Role = model.Role
            };
            return dto;
        }
    }
}