using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechnicalShop.Models;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {

        public AdminHomeController()
        {
        }

        public IActionResult Index()
        {
            string? ses = HttpContext.Session.GetString("user");
            AccountModel? user = null;

            if (!string.IsNullOrEmpty(ses))
            {
                user = JsonSerializer.Deserialize<AccountModel>(ses);
            }

            if (user != null && user.Role == "admin")
            {
                return View("Index");
            }
            else
            {
                return View("Login");
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.SetString("user", string.Empty);
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home", new { area = "Default" });
        }
    }
}