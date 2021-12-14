using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaladlog.Models;

namespace MySaladlog
{
    public class UsersController : Controller
    {
        private readonly SaladlogContext _context;
        private readonly IWebHostEnvironment _webHost;

        public UsersController(SaladlogContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        [HttpGet]
        public IActionResult Login()
        {
            List<Tag> tags = _context.Tags.ToList();

            ViewBag.listTags = tags;
            return View();

        }

        [HttpPost]
        public IActionResult Login(User users)
        {
            if (users == null)
            {
                return View(users);
            }

            var user = _context.Users
                .FirstOrDefault(m => m.UserName == users.UserName);
            if (user == null)
            {
                return View(users);
            }
            if (Decrypt(user.Password) != users.Password)
            {

                return View(users);
            }
            SaveSession(user);

            LoadArticleView();
            List<Tag> tags = _context.Tags.ToList();

            ViewBag.listTags = tags;
            return View("~/Views/Article/Feed.cshtml");

        }

        public IActionResult Index()
        {
            LoadArticleView();
            return View("Home");
        }


        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.IdTag == id);
            List<Tag> tags = _context.Tags.ToList();

            ViewBag.listTags = tags;
            return View(tag);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            user.Password = Encrypt(user.Password);
            if (ModelState.IsValid)
            {
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        string Encrypt(string data)
        {
            byte[] encryp = System.Text.Encoding.Unicode.GetBytes(data.ToString());
            return Convert.ToBase64String(encryp);
        }

        string Decrypt(string data)
        {
            byte[] decrypt = Convert.FromBase64String(data);

            return System.Text.Encoding.Unicode.GetString(decrypt); ;
        }


        public void SaveSession(User user)
        {

            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetInt32("IdUser", user.IdUser);

        }
        public void DeleteSession()
        {
            HttpContext.Session.Clear();
        }

        public void LoadArticleView()
        {
            List<Tag> tags = _context.Tags.ToList();
            ViewBag.listTags = tags;
            List<Article> articles = _context.Articles.ToList();
            List<User> users = _context.Users.ToList();

            var dataArticle = from a in articles
                              join u in users on a.IdUser equals u.IdUser
                              select new ArticleDataView(a.CreateDate, a.Title, FileContentPath(a.Path), u.FirstName + " " + u.LastName);

            ViewBag.ListArticlesFeed = dataArticle.ToList();
        }

        protected string FileContentPath(string fileName)
        {
            string filePath = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", fileName);
            string data = System.IO.File.ReadAllText(filePath);
            return data;
        }

        public IActionResult SignOuts()
        {
            DeleteSession();
            return View("~/Views/Home/Index.cshtml");
        }

    }
}