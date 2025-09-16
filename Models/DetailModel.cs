using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalShop.Models
{
    public class DetailModel
    {
        public ProductModel Products { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}