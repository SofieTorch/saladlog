using Microsoft.AspNetCore.Http;
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
            if (VerificationSession())
            {
                if (ModelState.IsValid)
                {
                    if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetInt32("IdUser") != null)
                    {
                        comments.IdUser = (short)HttpContext.Session.GetInt32("IdUser");
                        _context.Comments.Add(comments);
                        _context.SaveChanges();
                    }


                }

                return RedirectToAction("Index", "Article", new { id = comments.IdArticle });
            }
            else
            {

                return RedirectToAction("Login", "Users");
            }

        }

        public bool VerificationSession()
        {
            bool key = false;
            if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetInt32("IdUser") != null)
                key = true;

            return key;
        }
    }
}
