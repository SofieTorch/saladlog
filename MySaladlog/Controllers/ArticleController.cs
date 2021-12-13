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
        private readonly SaladlogContext _context;

        public ArticleController(SaladlogContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ArticlesByTag(short id)
        {
            List<Article> articles = _context.Articles.ToList();
            List<Tag> tags = _context.Tags.ToList();
            List<TagArticle> ta = _context.TagArticles.ToList();

            var articlesTag = (from art in articles
                              join tagart in ta
                              on art.IdArticle equals tagart.IdArticle
                              join tag in tags
                              on tagart.IdTag equals tag.IdTag
                              where tag.IdTag == id
                              select art);
            ViewBag.ArticlesTags = articlesTag;
            return View();
        }

        public IActionResult FindArticle(string article)
        {
            List<Article> articles = _context.Articles.ToList();
            var res = (from art in articles
                      where art.Title == article
                      select art);
            ViewBag.FindArt = res;
            return View();
        }

        public IActionResult TopAticles()
        {
            List<Article> articles = _context.Articles.ToList();
            List<LikeArticle> likeArt = _context.LikeArticles.ToList();

            var resp = (from art in articles
                        join la in likeArt 
                        on art.IdArticle equals la.IdArticle
                        select art).Take(10);
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
