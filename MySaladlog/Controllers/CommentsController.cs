using Microsoft.AspNetCore.Mvc;
using MySaladlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly SaladlogContext _context;

        public CommentsController(SaladlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Comment comments)
        {
            if (ModelState.IsValid)
            {
                _context.Comments.Add(comments);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Article", new { id = comments.IdArticle });
        }
    }
}
