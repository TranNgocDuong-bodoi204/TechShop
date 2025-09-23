using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;

namespace TechnicalShop.Models.DTO
{
    public class HomeViewDTO
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<BrandModel> Brands { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }

        public HomeViewDTO() { }

        public HomeViewDTO(List<ProductModel> product, List<CategoryModel> category, List<BrandModel> brand)
        {
            this.Products = product;
            this.Brands = brand;
            this.Categories = category;
        }
    }
}