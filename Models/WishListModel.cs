using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models
{
    public class WishListModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }
        public AccountModel Account { get; set; }

        [Required]
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public WishListModel(){}
        public WishListModel(AccountModel user, ProductModel product)
        {
            this.AccountId = user.Id;
            this.ProductId = product.Id;

            this.Account = user;
            this.Product = product;
        }

    }
}