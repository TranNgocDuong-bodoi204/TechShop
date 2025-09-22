using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechnicalShop.Repository;

namespace TechnicalShop.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly DBDataContext _context;

        public CommentController(DBDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments.ToListAsync();

            return View(comments);

        }
        public IActionResult approve(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                comment.status = 1;
                _context.Comments.Update(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult delete(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
            var comments = _context.Comments.ToList();
            return RedirectToAction("Index", comments);
        }
    }
    

}