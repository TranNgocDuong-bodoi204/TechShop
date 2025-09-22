using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalShop.Models.Enum;

namespace TechnicalShop.Helper
{
    public static class EnumToView
    {
        public static string OrdetToView(OrderStatus orderstatus)
        {
            string text = "";
            switch (orderstatus)
            {
                case OrderStatus.Pending:
                    text = "Chờ xác nhận";
                    break;
                case OrderStatus.Processing:
                    text = "Đang xử lý";
                    break;
                case OrderStatus.Shipping:
                    text = "Đang giao hàng";
                    break;
                case OrderStatus.Completed:
                    text = "Hoàn thành";
                    break;
                case OrderStatus.Cancelled:
                    text = "Đã hủy";
                    break;
                default:
                    text = "Không xác định";
                    break;
            }
            return text;
        }
        public static string PayemtStatusToView(PaymentStatus paymentStatus)
        {
            string text = "";
            switch (paymentStatus)
            {
                case PaymentStatus.Unpaid:
                    text = "Chưa thanh toán";
                    break;
                case PaymentStatus.Paid:
                    text = "Đã thanh toán";
                    break;
            }
            return text;
        }
        public static string PaymentMethodToView(PaymentMethod paymentMethod)
        {
            string text = "";
            switch (paymentMethod)
            {
                case PaymentMethod.COD:
                    text = "Trực tiếp";
                    break;
                case PaymentMethod.Bank:
                    text = "Chuyển khoản";
                    break;
            }
            return text;
        }
    }
}