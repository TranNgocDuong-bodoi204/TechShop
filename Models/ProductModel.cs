using System.ComponentModel.DataAnnotations;
using TechnicalShop;
namespace TechnicalShop.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; } = 0;
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public string Image { get; set; }
        public string status { get; set; } = "Còn hàng";
    }
}
