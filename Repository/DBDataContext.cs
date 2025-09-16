using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Models;

namespace TechnicalShop.Repository
{
    public class DBDataContext : DbContext
    {
        public DBDataContext(DbContextOptions<DBDataContext> options)
        : base(options)
        {
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<AccountModel> Account { get; set; }
        public DbSet<InventoryModel> Inventories { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
    }
}
