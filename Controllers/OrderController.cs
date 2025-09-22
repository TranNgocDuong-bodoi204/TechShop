using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Models;
using TechnicalShop.Models.DTO;
using TechnicalShop.Models.Enum;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{

    public class OrderController : Controller
    {
        private readonly DBDataContext _context;

        public OrderController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? userJson = HttpContext.Session.GetString("user");
            List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();
            if (!string.IsNullOrEmpty(userJson))
            {
                AccountModel? user = JsonSerializer.Deserialize<AccountModel>(userJson);

                List<Order> orders = await _context.Orders.Where(x => x.AccountId == user.Id).ToListAsync();
                foreach (var order in orders)
                {
                    OrderDetailDTO orderDetailDTO = new OrderDetailDTO();
                    orderDetailDTO.id = order.OrderId;
                    orderDetailDTO.orderAt = order.CreatedDate;
                    orderDetailDTO.orderStatus = order.orderStatus;
                    orderDetailDTO.paymentStatus = order.paymentStatus;
                    orderDetailDTO.CustomerName = order.CustomerName;
                    orderDetailDTO.Email = order.Email;
                    orderDetailDTO.Phone = order.Phone;
                    orderDetailDTO.Address = order.Address;

                    List<OrderItem> orderItems = await _context.OrderItems.Where(i => i.OrderId == order.OrderId).ToListAsync();
                    List<OrderItemDetail> orderItemDetails = new List<OrderItemDetail>();
                    foreach (var item in orderItems)
                    {
                        string? image = await _context.Products.Where(x => x.Id == item.ProductId).Select(i => i.Image).FirstOrDefaultAsync();
                        string? description = await _context.Products.Where(x => x.Id == item.ProductId).Select(i => i.Description).FirstOrDefaultAsync();

                        OrderItemDetail orderItemDetail = new OrderItemDetail();
                        orderItemDetail.ProductName = item.ProductName;
                        orderItemDetail.Description = description;
                        orderItemDetail.Price = item.Price;
                        orderItemDetail.Image = image;
                        orderItemDetail.Quantity = item.Quantity;

                        orderItemDetails.Add(orderItemDetail);
                    }
                    orderDetailDTO.orderDetails = orderItemDetails;
                    orderDetailDTO.TotalAmount = order.TotalAmount;

                    orderDetailDTOs.Add(orderDetailDTO);
                }
            }
            return View(orderDetailDTOs);
        }
        public async Task<IActionResult> Filter(
        string? code,
        OrderStatus? status,
        PaymentStatus? payment,
        DateTime? from,
        DateTime? to)
    {
        var result = new List<OrderDetailDTO>();

        string? userJson = HttpContext.Session.GetString("user");
        if (string.IsNullOrEmpty(userJson))
            return View("Index", result);

        var user = JsonSerializer.Deserialize<AccountModel>(userJson);
        if (user == null)
            return View("Index", result);

        IQueryable<Order> query = _context.Orders.Where(o => o.AccountId == user.Id);

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(o => o.OrderId.ToString().Contains(code));

        if (status.HasValue)
            query = query.Where(o => o.orderStatus == status.Value);

        if (payment.HasValue)
            query = query.Where(o => o.paymentStatus == payment.Value);

        if (from.HasValue)
            query = query.Where(o => o.CreatedDate >= from.Value.Date);

        if (to.HasValue)
        {
            var toInclusive = to.Value.Date.AddDays(1);
            query = query.Where(o => o.CreatedDate < toInclusive);
        }

        var orders = await query
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();

        if (orders.Count == 0)
        {
            // đẩy lại các giá trị filter để bind vào filter UI
            ViewBag.Code = code;
            ViewBag.Status = status?.ToString();
            ViewBag.Payment = payment?.ToString();
            ViewBag.From = from?.ToString("yyyy-MM-dd");
            ViewBag.To = to?.ToString("yyyy-MM-dd");
            return View("Index", result);
        }

        var orderIds = orders.Select(o => o.OrderId).ToList();

        var items = await _context.OrderItems
            .Where(i => orderIds.Contains(i.OrderId))
            .Select(i => new
            {
                i.OrderId,
                i.ProductId,
                i.ProductName,
                i.Price,
                i.Quantity
            })
            .ToListAsync();

        var productIds = items.Select(i => i.ProductId).Distinct().ToList();

        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Image, p.Description })
            .ToListAsync();

        var productLookup = products.ToDictionary(p => p.Id, p => p);

        foreach (var order in orders)
        {
            var dto = new OrderDetailDTO
            {
                id = order.OrderId,
                orderAt = order.CreatedDate,
                orderStatus = order.orderStatus,
                paymentStatus = order.paymentStatus,
                CustomerName = order.CustomerName,
                Email = order.Email,
                Phone = order.Phone,
                Address = order.Address,
                TotalAmount = order.TotalAmount,
                orderDetails = new List<OrderItemDetail>()
            };

            foreach (var it in items.Where(x => x.OrderId == order.OrderId))
            {
                productLookup.TryGetValue(it.ProductId, out var prod);
                dto.orderDetails.Add(new OrderItemDetail
                {
                    ProductName = it.ProductName,
                    Description = prod?.Description,
                    Image = prod?.Image,
                    Price = it.Price,
                    Quantity = it.Quantity
                });
            }

            result.Add(dto);
        }

        // đẩy lại các giá trị filter để bind vào filter UI
        ViewBag.Code = code;
        ViewBag.Status = status?.ToString();
        ViewBag.Payment = payment?.ToString();
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");

        return View("Index", result);
    }
    }
}