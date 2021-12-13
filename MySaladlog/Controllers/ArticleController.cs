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

        public IActionResult New()
        {
            Article newArticle = new Article();
            newArticle.Title = "Sin título";
            newArticle.MdContent = "**Holiii** esto es una prueba";
            return View(newArticle);
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
            ViewBag.ListArticlesIndex = articlesTag;
            return View();
        }


        public IActionResult Feed()
        {
            LoadArticleView();
            return View();
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

    }
}
