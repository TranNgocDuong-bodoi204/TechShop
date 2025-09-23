using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models.DTO
{
    public class WishlistDTO
    {
        public int productID { get; set; }
        public int userID{ get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public WishlistDTO()
        {}
        public WishlistDTO(ProductModel model,int userId)
        {
            this.userID = userId;

            this.productID = model.Id;
            this.productName = model.Name;
            this.productImage = model.Image;
            this.Description = model.Description;
            this.Price = model.Price;
        }
    }
}