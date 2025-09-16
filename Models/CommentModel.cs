using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models;

namespace TechnicalShop.Models
{
    public class CommentModel
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        public string userComment { get; set; } 
        public int status { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        public int AccountId { get; set; }
        public AccountModel Account { get; set; }
        
    }
}