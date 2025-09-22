using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Models;
using TechnicalShop.Repository;

namespace TechnicalShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly DBDataContext _context;
        public AccountController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Account.ToListAsync();

            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string role)
        {
            if (role == "all")
            {
                return View(await _context.Account.ToListAsync());
            }

            IEnumerable<AccountModel> accounts = await _context.Account.Where(acc => acc.Role == role).ToListAsync();
            return View(accounts);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            AccountModel? account = await _context.Account.FindAsync(id);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AccountModel account)
        {
            var acc = await _context.Account.FindAsync(account.Id);

            acc.Username = account.Username;
            acc.Email = account.Email;
            acc.PhoneNumber = account.PhoneNumber;
            acc.Address = account.Address;
            acc.Role = account.Role;
            await _context.SaveChangesAsync();
            return View("Index", await _context.Account.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Create(AccountModel account)
{
    bool checkUserName = await _context.Account.AnyAsync(a => a.Username == account.Username);
    if (checkUserName)
    {
        TempData["NotificationMessage"] = "Tên tài khoản đã được sử dụng!";
        TempData["NotificationType"] = "error";
        return RedirectToAction("Create", "Account", new { area = "admin" });
    }


    bool checkEmail = await _context.Account.AnyAsync(a => a.Email == account.Email);
    if (checkEmail)
    {
        TempData["NotificationMessage"] = "Email đã được sử dụng!";
        TempData["NotificationType"] = "error";
        return RedirectToAction("Create", "Account", new { area = "admin" });
    }
    _context.Account.Add(account);
    await _context.SaveChangesAsync();

    TempData["NotificationMessage"] = "Thêm tài khoản thành công!";
    TempData["NotificationType"] = "success";

    return RedirectToAction("Index", "Account", new { area = "admin" });
}


        
        
    }
}