using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public UsersController(SaladlogContext context)
        {
            _context = context;
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
           
            HttpContext.Session.SetString("UserName",user.UserName );
            HttpContext.Session.SetInt32("IdUser", user.IdUser);

        }
        public void DeleteSession()
        {
            HttpContext.Session.Clear();
        }

        public void LoadArticleView()
        {
            List<Article> articles = _context.Articles.ToList();
            List<User> users = _context.Users.ToList();

            var dataArticle = from a in articles
                              join u in users on a.IdUser equals u.IdUser
                              select new ArticleDataView(a.CreateDate, a.Title, "Hamas gano combinando una fuerte resistencia contra la ocupacion militar con la creacion de organizaciones sociales de base y de servicio a los pobres, una plataforma y una practica que probablemente haria ganar votos en cualquier lugar. La victoria electoral de Hamas es ominosa pero comprensible, a la luz de los acontecimientos. Es enteramente justo describir a Hamas como fundamentalista, extremista y violentista, y como una seria amenaza a la paz y a un acuerdo politicamente justo. Sin embargo, es útil recordar que en aspectos importantes, Hamas no es tan extremista como otros. Por ejemplo, declara que estaría de acuerdo con una tregua con Israel sobre la base de la frontera reconocida a nivel internacional antes de la guerra arabe-israeli de junio de l967. ..", u.FirstName + " +" + u.LastName);
                              
            ViewBag.ListArticlesIndex = dataArticle.ToList();
        }


        public IActionResult SignOuts()
        {
            DeleteSession();
            return View("~/Views/Home/Index.cshtml");
        }

    }
}
