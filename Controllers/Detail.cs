using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalShop.Models;
using TechnicalShop.Repository;

namespace TechShopOnline.Controllers
{
    public class Detail : Controller
    {
        private readonly DBDataContext _context;
        public Detail(DBDataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            ProductModel? product = await _context.Products.FindAsync(id);
            List<CommentModel> comments = await _context.Comments.Where(x => x.ProductId == id).ToListAsync();

            product.Brand = await _context.Brands.FindAsync(product.BrandId);

            product.Category = await _context.Categories.FindAsync(product.CategoryId);

            DetailModel detailModel = new DetailModel
            {
                Products = product,
                Comments = comments
            };


            if (product == null) return RedirectToAction("Index");

            return View(detailModel);
        }

        public async Task<IActionResult> addComment(int Rating, int ProductId, int AccountId, string Content)
        {
            CommentModel comment = new CommentModel
            {
                Rating = Rating,
                ProductId = ProductId,
                AccountId = AccountId,
                Content = Content,
                CreatedAt = DateTime.Now
            };
            var user = JsonSerializer.Deserialize<AccountModel>(HttpContext.Session.GetString("user"));

            var account = await _context.Account.FindAsync(user.Id);
            var product = await _context.Products.FindAsync(ProductId);

            comment.Account = account;
            comment.Product = product;

            comment.userComment = account.Username;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = ProductId });
        }

        public IActionResult delete(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", new { id = comment.ProductId });
        }


    }
}
