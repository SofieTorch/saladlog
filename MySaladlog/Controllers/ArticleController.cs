using Microsoft.AspNetCore.Mvc;
using MySaladlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            Article newArticle = new Article();
            newArticle.Title = "Sin título";
            newArticle.MdContent = "**Holiii** esto es una prueba";
            return View(newArticle);
        }
    }
}
