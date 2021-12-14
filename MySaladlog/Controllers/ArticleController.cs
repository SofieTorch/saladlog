using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaladlog.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Westwind.AspNetCore.Markdown;


namespace MySaladlog.Controllers
{
    public class ArticleController : Controller
    {

        private readonly SaladlogContext _context;
        private readonly IWebHostEnvironment _webHost;
        Article article;

        public ArticleController(IWebHostEnvironment webHost, SaladlogContext context)
        {
            _webHost = webHost;
            _context = context;
        }

        public IActionResult Index(short id)
        {
            List<Tag> tags = _context.Tags.ToList();
            ViewBag.listTags = tags;
            article = _context.Articles
                .Include(a => a.Comments)
                .ThenInclude(c => c.IdUserNavigation)
                .Where(a => a.IdArticle == id).First();

            return View(article);
        }


        public IActionResult FindArticle(string article)
        {
            List<Tag> tags = _context.Tags.ToList();
            ViewBag.listTags = tags;
            List<Article> articles = _context.Articles.ToList();
            List<User> users = _context.Users.ToList();

            var dataArticle = from a in articles
                              join u in users on a.IdUser equals u.IdUser
                              where a.Title.Equals(article)
                              select new ArticleDataView(a.IdArticle, a.CreateDate, a.Title, FileContentPath(a.Path), u.FirstName + " " + u.LastName);
            ViewBag.ListArticlesFeed = dataArticle.ToList();
            

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

        public IActionResult Edit(short id = 1)
        {
            List<Tag> tags = _context.Tags.ToList();
            ViewBag.listTags = tags;
            article = _context.Articles.Find(id);
            article.IdArticle = id;
            string path = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", article.Path);
            article.MdContent = System.IO.File.ReadAllText(path);
            return View("New", article);
        }

        public IActionResult New()
        {
            if(VerificationSession())
            {
                article = new Article();
                article.Title = "Mi título genial";
                List<Tag> tags = _context.Tags.ToList();
                ViewBag.listTags = tags;
                return View(article);
            }
            else
            {

                return RedirectToAction("Login","Users");
            }
            
        }

        public bool VerificationSession()
        {
            bool key = false;
            if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetInt32("IdUser") != null)
                key = true;

            return key;
        }

        [HttpPost]
        public async Task<IActionResult> New(IFormFile file, Article obj)
        {
            List<Tag> tags = _context.Tags.ToList();
            ViewBag.listTags = tags;
            string extension = Path.GetExtension(file.FileName);
            if (extension == ".jpeg" || extension == ".jpg" || extension == ".png")
            {
                string newFileName = await SaveImage(file);
                string updatedContent = obj.MdContent + $"  \n![{file.FileName}](../../images/{newFileName})  \n";
                ModelState.SetModelValue("MdContent", new ValueProviderResult(updatedContent));
                ViewData["Message"] = null;
            }
            else
            {
                ViewData["Message"] = "Ups! Formato de imagen inválido";
            }

            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> PostArticle(Article obj)
        {

            obj.IdUser = (short)HttpContext.Session.GetInt32("IdUser");
            if (obj.IdArticle == 0) await AddArticle(obj);
            else await SaveArticleChanges(obj);

            _context.SaveChanges();
            return RedirectToAction("Index", new { id = obj.IdArticle });
        }

        private async Task AddArticle(Article article)
        {
            string fileName = GetArticleFileName(article.Title);
            string filePath = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", fileName);
            await System.IO.File.WriteAllTextAsync(filePath, article.MdContent);
            article.Path = fileName;
            _context.Articles.Add(article);
        }

        private async Task SaveArticleChanges(Article article)
        {
            Article articleDb = _context.Articles.Find(article.IdArticle);
            articleDb.Title = article.Title;
            string filePath = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", article.Path);
            await System.IO.File.WriteAllTextAsync(filePath, article.MdContent);
        }

        private string GetArticleFileName(string articleTitle)
        {
            string fileName;
            int matchingCount;
            do
            {
                string sanitizedTitle = "_" + RemoveInvalidFileNameCharacters(articleTitle).Replace(' ', '_');
                fileName = Guid.NewGuid().ToString().Replace('-', '_') + sanitizedTitle + ".md";
                var helper = _context.Articles.Where(article => article.Path == fileName);
                matchingCount = helper.Count();
            }
            while (matchingCount > 0);

            return fileName;
        }

        private string RemoveInvalidFileNameCharacters(string filename)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, "_");
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            string newFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            newFileName = newFileName.Replace(' ', '_').Replace('-', '_');
            string filePath = Path.Combine(_webHost.WebRootPath, "images", newFileName);

            FileStream stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return newFileName;
        }

        public int idPublic;
        public IActionResult ArticlesByTag(short id)

        {
            List<Article> articles = _context.Articles.ToList();
            List<Tag> tags = _context.Tags.ToList();
            List<TagArticle> ta = _context.TagArticles.ToList();
            List<User> users = _context.Users.ToList();

            var articlesTag = (from art in articles
                               join tagart in ta
                               on art.IdArticle equals tagart.IdArticle
                               join tag in tags
                               on tagart.IdTag equals tag.IdTag
                               join u in users on art.IdUser equals u.IdUser
                               where tag.IdTag == id
                               select new ArticleDataView(art.IdArticle,art.CreateDate, art.Title, FileContentPath(art.Path), u.FirstName + " " + u.LastName));
            ViewBag.ListArticlesFeed = articlesTag.ToList() ;

           
            ViewBag.listTags = tags;
            return View();
        }

     
        public IActionResult Feed()
        {
            LoadArticleView();
            return View();
        }

        public void LoadArticleView()
        {
            List<Tag> tags = _context.Tags.ToList();
            ViewBag.listTags = tags;
            List<Article> articles = _context.Articles.ToList();
            List<User> users = _context.Users.ToList();

            var dataArticle = from a in articles
                              join u in users on a.IdUser equals u.IdUser
                              select new ArticleDataView(a.IdArticle,a.CreateDate, a.Title, FileContentPath(a.Path), u.FirstName + " " + u.LastName);

            ViewBag.ListArticlesFeed = dataArticle.ToList();
        }
        protected string FileContentPath(string fileName)
        {
            string filePath = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", fileName);
            string data = System.IO.File.ReadAllText(filePath);
            return data;
        }
    }
}
