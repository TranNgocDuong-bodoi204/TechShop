using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Areas.Admin.Model.DTO;
using TechnicalShop.Models;
using TechnicalShop.Models.DTO;
using TechnicalShop.Models.Enum;
using TechnicalShop.Repository;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly DBDataContext _context;
        public OrderController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            OrderDashboardDTO orderDTO = new OrderDashboardDTO();

            List<Order> orders = new List<Order>();
            orders = await _context.Orders.ToListAsync();
            CaculateDashboardNumber(orders, orderDTO);
            return View(orderDTO);
        }

        // accept order action
        public async Task<IActionResult> Accept()
        {
            var order = await _context.Orders.Where(x =>x.orderStatus == OrderStatus.Pending).ToListAsync();
            List<AcceptDTO> acceptDTOs = new List<AcceptDTO>();

            foreach (var item in order)
            {
                AcceptDTO acceptDTO = new AcceptDTO(item);
                acceptDTOs.Add(acceptDTO);
            }
            return View(acceptDTOs);
        }
        [HttpPost]
        public IActionResult Accept(string orders)
        {
            if (string.IsNullOrEmpty(orders) || orders == null)
            {
                Console.WriteLine("orders null");
                return View(new List<AcceptDTO>());
            }
            List<Order>? orderNA = JsonSerializer.Deserialize<List<Order>>(orders);

            List<AcceptDTO> acceptDTOs = new List<AcceptDTO>();
            foreach (var item in orderNA)
            {
                AcceptDTO dto = new AcceptDTO(item);

                acceptDTOs.Add(dto);
            }
            return View(acceptDTOs);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(string orders, int id)
        {
            List<AcceptDTO>? acceptDTOs = JsonSerializer.Deserialize<List<AcceptDTO>>(orders);
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);

            AcceptDTO acceptDTO = new AcceptDTO();
            foreach (var dto in acceptDTOs)
            {
                acceptDTO = dto;
            }
            acceptDTOs.Remove(acceptDTO);

            order.orderStatus = OrderStatus.Processing;
            await _context.SaveChangesAsync();

            return View("Accept",acceptDTOs);
        }

        public IActionResult Update(string orders)
        {
            List<Order>? orderL = JsonSerializer.Deserialize<List<Order>>(orders);
            List<UpdateDTO> dto = new List<UpdateDTO>();

            foreach (var order in orderL)
            {
                UpdateDTO updateDTO = new UpdateDTO(order);
                dto.Add(updateDTO);
            }

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptUpdate([FromForm] string updateList)
        {
            if (!string.IsNullOrEmpty(updateList))
            {
                List<UpdateDTO>? updates = JsonSerializer.Deserialize<List<UpdateDTO>>(updateList);

                foreach (var item in updates)
                {
                    var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == item.id);
                    order.orderStatus = item.orderStatus;
                    order.paymentStatus = item.paymentStatus;
                }
                await _context.SaveChangesAsync();
            }

            OrderDashboardDTO orderDTO = new OrderDashboardDTO();
            List<Order> orders = new List<Order>();
            orders = await _context.Orders.ToListAsync();
            CaculateDashboardNumber(orders, orderDTO);

            List<UpdateDTO> dto = new List<UpdateDTO>();
            foreach (var item in orders)
            {
                UpdateDTO updateDTO = new UpdateDTO(item);
                dto.Add(updateDTO);
            }
            TempData["NotificationMessage"] = "Cập nhật dữ liệu đơn hàng thành công";
            TempData["NotificationType"] = "success";
            return View("Update", dto);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _context.Orders.Where(x => x.OrderId == id).FirstOrDefaultAsync();

            order.orderStatus = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
            var orders = await _context.Orders.Where(x =>x.orderStatus == OrderStatus.Pending).ToListAsync();
            List<AcceptDTO> acceptDTOs = new List<AcceptDTO>();

            foreach (var item in orders)
            {
                AcceptDTO acceptDTO = new AcceptDTO(item);
                acceptDTOs.Add(acceptDTO);
            }
            return View("Accept", acceptDTOs);
        }

        // order detail
        [HttpPost]
        public async Task<IActionResult> ViewDetail(int id)
        {
            var order = await _context.Orders.Where(x => x.OrderId == id).FirstOrDefaultAsync();

            List<OrderItemDetail> orderItemDetails = new List<OrderItemDetail>();

            var orderItems = await _context.OrderItems.Where(x => x.OrderId == order.OrderId).ToListAsync();
            foreach (var item in orderItems)
            {
                OrderItemDetail itemDetail = new OrderItemDetail();
                itemDetail.ProductName = item.ProductName;
                itemDetail.Price = item.Price;
                itemDetail.Quantity = item.Quantity;
                itemDetail.Image = await _context.Products.Where(x => x.Id == item.ProductId).Select(i => i.Image).FirstOrDefaultAsync() ?? "avarta_1.jpg";
                itemDetail.Description = await _context.Products.Where(x => x.Id == item.ProductId).Select(x => x.Description).FirstOrDefaultAsync() ?? "không có";
                orderItemDetails.Add(itemDetail);
            }

            OrderDetailDTO dto = new OrderDetailDTO();
            dto.SetOrderDetailInfo(order, orderItemDetails);

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _context.Orders.Where(x => x.OrderId == id).FirstOrDefaultAsync();

            List<OrderItemDetail> orderItemDetails = new List<OrderItemDetail>();

            var orderItems = await _context.OrderItems.Where(x => x.OrderId == order.OrderId).ToListAsync();
            foreach (var item in orderItems)
            {
                OrderItemDetail itemDetail = new OrderItemDetail();
                itemDetail.ProductName = item.ProductName;
                itemDetail.Price = item.Price;
                itemDetail.Quantity = item.Quantity;
                itemDetail.Image = await _context.Products.Where(x => x.Id == item.ProductId).Select(i => i.Image).FirstOrDefaultAsync() ?? "avarta_1.jpg";
                itemDetail.Description = await _context.Products.Where(x => x.Id == item.ProductId).Select(x => x.Description).FirstOrDefaultAsync() ?? "không có";
                orderItemDetails.Add(itemDetail);
            }

            OrderDetailDTO dto = new OrderDetailDTO();
            dto.SetOrderDetailInfo(order, orderItemDetails);

            return View(dto);
        }

        public async Task<IActionResult> ViewOrder()
        {
            var order = await _context.Orders.ToListAsync();
            return View(order);
        }

        private void CaculateDashboardNumber(List<Order> orders, OrderDashboardDTO dto)
        {
            foreach (var item in orders)
            {
                dto.total += 1;
                if (item.orderStatus == OrderStatus.Pending)
                {
                    dto.notaccepted += 1;
                    dto.orderNotAccept.Add(item);
                }
                else if (item.orderStatus == OrderStatus.Processing)
                {
                    dto.orderAccepted.Add(item);
                    dto.accepted += 1;
                }
                else if (item.orderStatus == OrderStatus.Shipping)
                {
                    dto.orderAccepted.Add(item);
                    dto.shipping += 1;
                }
                if (item.orderStatus == OrderStatus.Completed)
                {
                    dto.orderAccepted.Add(item);
                    dto.completed += 1;
                }
                if (item.orderStatus == OrderStatus.Cancelled)
                {
                    dto.canceled += 1;
                }
            }
        }
    }
}