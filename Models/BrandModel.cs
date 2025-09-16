﻿using System.ComponentModel.DataAnnotations;

namespace TechnicalShop.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
