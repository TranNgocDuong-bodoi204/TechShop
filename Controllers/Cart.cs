using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using TechnicalShop.Models;
using TechnicalShop.Models.ViewModels;
using TechnicalShop.Repository;

namespace TechShopOnline.Controllers
{
    public class Cart : Controller
    {
        private readonly DBDataContext _context;
        public Cart(DBDataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string? cartitem = HttpContext.Session.GetString("Cart");

            List<CartItemModel> cart = string.IsNullOrEmpty(cartitem)
                ? new List<CartItemModel>()
                : JsonSerializer.Deserialize<List<CartItemModel>>(cartitem)!;

            CartItemViewModel cartView = new CartItemViewModel
            {
                listCartItem = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(cartView);
        }

        public IActionResult Add(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            string? cartitem = HttpContext.Session.GetString("Cart");

            List<CartItemModel> cart;

            if (cartitem == null)
            {
                cart = new List<CartItemModel>();
            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartitem)!;
            }

            var productIncart = cart.FirstOrDefault(x => x.ProductId == id);
            if (productIncart == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                productIncart.Quantity++;
            }
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            return RedirectToAction("Index","Home");
        }

        public IActionResult Remove(int id)
        {
            string? cartitem = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartitem))
            {
                return RedirectToAction("Index");
            }

            List<CartItemModel> cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartitem)!;

            var productIncart = cart.FirstOrDefault(x => x.ProductId == id);
            if (productIncart != null)
            {
                cart.Remove(productIncart);
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }
            TempData["success"] = "Xóa sản phẩm khỏi giỏ hàng thành công!";

            return RedirectToAction("Index");
        }

        public IActionResult Increase(int id)
        {
            string? cartitem = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartitem))
            {
                return RedirectToAction("Index");
            }

            List<CartItemModel> cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartitem)!;

            var productIncart = cart.FirstOrDefault(x => x.ProductId == id);
            if (productIncart != null)
            {
                productIncart.Quantity++;
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }
            TempData["success"] = "Tăng số lượng sản phẩm thành công!";

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            string? cartitem = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartitem))
            {
                return RedirectToAction("Index");
            }

            List<CartItemModel> cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartitem)!;

            var productIncart = cart.FirstOrDefault(x => x.ProductId == id);
            if (productIncart != null)
            {
                if (productIncart.Quantity > 1)
                {
                    productIncart.Quantity--;
                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
                }
                else
                {
                    cart.Remove(productIncart);
                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
                }
            }
            TempData["success"] = "Giảm số lượng sản phẩm thành công!";

            return RedirectToAction("Index");
        }
    }
}
