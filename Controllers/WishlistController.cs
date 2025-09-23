using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Migrations;
using TechnicalShop.Models;
using TechnicalShop.Models.DTO;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly DBDataContext _context;
        public WishlistController(DBDataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Require", "Account");
            }
            AccountModel? user = JsonSerializer.Deserialize<AccountModel>(userJson);
            List<WishListModel> wishListModels = await _context.WishLists.Where(x => x.AccountId == user.Id).ToListAsync();
            List<WishlistDTO> dto = new List<WishlistDTO>();
            foreach (var item in wishListModels)
            {
                ProductModel? product = await _context.Products.FindAsync(item.ProductId);
                WishlistDTO wishlistDTO = new WishlistDTO(product, user.Id);
                dto.Add(wishlistDTO);
            }
            return View(dto);
        }

        public async Task<IActionResult> Add(int id)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Require", "Account");
            }
            bool isDup = false;
            AccountModel? user = JsonSerializer.Deserialize<AccountModel>(userJson);
            AccountModel? userModel = await _context.Account.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            List<WishListModel> whislist = await _context.WishLists.Where(x => x.AccountId == user.Id).ToListAsync();
            foreach (var wish in whislist)
            {
                if (wish.ProductId == id)
                {
                    isDup = true;
                }
            }

            if (!isDup)
            {
                ProductModel? productModel = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
                WishListModel wishListModel = new WishListModel(userModel, productModel);
                await _context.WishLists.AddAsync(wishListModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Delete(int id)
        {
            string? userJson = HttpContext.Session.GetString("user");
            AccountModel? user = JsonSerializer.Deserialize<AccountModel>(userJson);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var wishToDelete = await _context.WishLists
                                    .FirstOrDefaultAsync(x => x.AccountId == user.Id && x.ProductId == id);

            if (wishToDelete != null)
            {
                _context.WishLists.Remove(wishToDelete);
                await _context.SaveChangesAsync();
            }
            var userWishlist = await _context.WishLists
                                        .Where(x => x.AccountId == user.Id)
                                        .Include(x => x.Product)
                                        .ToListAsync();

            List<WishlistDTO> wishlistDTOs = userWishlist
                                            .Select(w => new WishlistDTO(w.Product, user.Id))
                                            .ToList();

            return View("Index", wishlistDTOs);
        }
        public async Task<IActionResult> AddCart(int id)
        {
            AddToCart(id);
            string? userJson = HttpContext.Session.GetString("user");
            AccountModel? user = JsonSerializer.Deserialize<AccountModel>(userJson);

            WishListModel? wishListModel = await _context.WishLists.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            _context.WishLists.Remove(wishListModel);
            await _context.SaveChangesAsync();

            List<WishListModel> wishListModels = await _context.WishLists.Where(x => x.AccountId == user.Id).ToListAsync();
            List<WishlistDTO> dto = new List<WishlistDTO>();

            foreach (var item in wishListModels)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                WishlistDTO wishlistDTO = new WishlistDTO(product, user.Id);

                dto.Add(wishlistDTO);
            }
            return View("Index", dto);
        }
        private void AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

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
        }

    }
}