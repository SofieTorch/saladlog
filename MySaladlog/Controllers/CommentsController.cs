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
        public IActionResult Index()
        {
            //List<Comment> comments = _context.Comments.ToList();
            //List<User> user = _context.Users.ToList();
            //int res = comments.Count();
            //var result = (from comm in comments
            //              join us in user
            //              on comm.IdUser equals us.IdUser
            //              select new CommentUser(comm.Comment1, us.FirstName, comm.DateComment, res)).ToList();

            //ViewBag.Comments = result;

            ViewBag.Comments = _context.Comments.ToList();

            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Comment comments)
        {
            comments.DateComment = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Comments.Add(comments);
                _context.SaveChanges();
                return View("../Views/Article/Index", comments.IdArticleNavigation);
            }

            return View("../Views/Article/Index", comments.IdArticleNavigation);
        }
    }
}
