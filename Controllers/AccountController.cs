using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Mapper;
using TechnicalShop.Models;
using TechnicalShop.Models.DTO;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBDataContext _context;
        public AccountController(DBDataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? userJosn = HttpContext.Session.GetString("user");

            if (userJosn != null)
            {
                var user = JsonSerializer.Deserialize<AccountModel>(userJosn);
                if (user.Role == "admin")
                {
                    return RedirectToAction("Index", "Product", new { area = "Admin" });
                }


            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string textLogin, string password)
        {
            var user = await _context.Account.FirstOrDefaultAsync(x => (x.Username == textLogin && x.Password == password) || x.Email == textLogin && x.Password == password);

            if (user != null)
            {
                string userJson = JsonSerializer.Serialize(user);

                HttpContext.Session.SetString("user", userJson);
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Sai thông tin username hoặc mật khẩu";
            return View();
        }
        public IActionResult Register()
        {
            return View(new AccountModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountModel account)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Account
                    .FirstOrDefaultAsync(x => x.Username == account.Username || x.Email == account.Email);

                if (user != null)
                {
                    string error = "";
                    if (account.Username == user.Username)
                    {
                        error += "Username đã được sử dụng.";
                    }
                    if (account.Email == user.Email)
                    {
                        error += "Email đã được sử dụng.";
                    }

                    TempData["error"] = error;
                    return View(account);
                }

                account.Role = "user";
                account.CreatedAt = DateTime.Now;

                _context.Account.Add(account);
                _context.SaveChanges();

                return RedirectToAction("Index", "Account");
            }
            return View(account);
        }

        public IActionResult AccountInfo()
        {
            string? userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }
            AccountModel? userModel = JsonSerializer.Deserialize<AccountModel>(userJson);

            if (userModel == null)
            {
                return RedirectToAction("Login", "Account");
            }

            AccountToDTO dto = new AccountToDTO();
            AccountViewDTO accountDTO = dto.ToDTO(userModel);

            return View(accountDTO);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }
        
    }
}