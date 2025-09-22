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
using TechnicalShop.Models.Enum;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{
    public class CheckoutController : Controller
    {
        private DBDataContext _context;

        public CheckoutController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? cartJson = HttpContext.Session.GetString("Cart");
            string? userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(cartJson))
            {
                return View(new CheckoutViewDTO());
            }

            List<CartItemModel>? cart = null;
            UserModel? user = null;
            decimal total = 0;

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);
            }
            if (!string.IsNullOrEmpty(userJson))
            {
                user = JsonSerializer.Deserialize<UserModel>(userJson);
            }

            foreach (var item in cart) total += item.Price * item.Quantity;
            Console.WriteLine("total: " + total);

            CheckoutViewDTO checkoutView = new CheckoutViewDTO(cart, total, user.Id);
            return View(checkoutView);
        }
        public async Task<IActionResult> CheckOut(string emailReused, int AccountId ,string Email,
        string CustomerName, string Address, string Phone,PaymentMethod paymentMethod, decimal TotalAmount,
        string cartItemJson)
        {
            List<CartItemModel>? cartItems = JsonSerializer.Deserialize<List<CartItemModel>>(cartItemJson);
            Order order = new Order();
            CheckoutSuccess checkoutViewSuccess = new CheckoutSuccess();
            if (paymentMethod == PaymentMethod.COD)
            {
                var account = await _context.Account.FirstOrDefaultAsync(acc => acc.Id == AccountId);
                //order
                if (emailReused != null)
                {
                    order.Email = account.Email;
                }
                else
                {
                    order.Email = Email;
                }
                order.AccountId = account.Id;
                order.CustomerName = CustomerName;
                order.Address = Address;
                order.Phone = Phone;
                order.PaymentMethod = paymentMethod;
                order.TotalAmount = TotalAmount;
                order.CreatedDate = DateTime.Now;
                order.paymentStatus = PaymentStatus.Unpaid;
                order.orderStatus = OrderStatus.Pending;

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                // order item
                foreach (var orderitem in cartItems)
                {
                    OrderItem orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = orderitem.ProductId,
                        ProductName = orderitem.ProductName,
                        Price = orderitem.Price,
                        Quantity = orderitem.Quantity,
                        Total = orderitem.Total
                    };

                    await _context.OrderItems.AddAsync(orderItem);
                }
                await _context.SaveChangesAsync();
                // return view
                checkoutViewSuccess.CustomerName = CustomerName;
                checkoutViewSuccess.TotalAmount = TotalAmount;
                checkoutViewSuccess.paymentMethod = paymentMethod;

                return View("Success", checkoutViewSuccess);
            }
            else if (paymentMethod == PaymentMethod.Bank)
            {
                Console.WriteLine("Bank hehehe");

            }

            return View("Index", new CheckoutViewDTO());
        }
    }
}