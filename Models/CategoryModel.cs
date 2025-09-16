using System.ComponentModel.DataAnnotations;

namespace TechnicalShop.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên danh mục ít nhất 4 ký tự")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
