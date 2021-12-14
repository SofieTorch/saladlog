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

namespace MySaladlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly SaladlogContext _context;
        Article article;

        public ArticleController(IWebHostEnvironment webHost, SaladlogContext context)
        {
            _webHost = webHost;
            _context = context;
        }

        public IActionResult Index(short id = 1)
        {
            article = _context.Articles
                .Include(a => a.Comments)
                .ThenInclude(c => c.IdUserNavigation)
                .Where(a => a.IdArticle == id).First();

            return View(article);
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

        public IActionResult Edit(short id = 1)
        {
            article = _context.Articles.Find(id);
            article.IdArticle = id;
            string path = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", article.Path);
            article.MdContent = System.IO.File.ReadAllText(path);
            return View("New", article);
        }

        public IActionResult New()
        {
            List<SelectListItem> tagList = _context.Tags.ToList()
                .ConvertAll(t => new SelectListItem()
                {
                    Text = t.TagName,
                    Value = t.IdTag.ToString(),
                    Selected = false
                }
            );

            ViewBag.tagList = _context.Tags.ToList();

            article = new Article();
            article.Title = "Mi título genial";
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> New(IFormFile file, Article obj)
        {
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
            obj.IdUser = 1; // TODO: Change to current user Id

            if (obj.IdArticle == 0) await AddArticle(obj);
            else await SaveArticleChanges(obj);

            _context.SaveChanges();
            return RedirectToAction("Index", new { id = obj.IdArticle });
        }

        [HttpPost]
        public IActionResult AddTag(Tag tag)
        {
            //obj.TagArticles.Add(new TagArticle { IdTagNavigation = tag, IdTag = tag.IdTag });
            //ModelState.Clear();
            var tagArticles = ModelState["TagArticles"] as ICollection<TagArticle>;
            tagArticles.Add(new TagArticle { IdTagNavigation = tag, IdTag = tag.IdTag });
            //ModelState.SetModelValue("TagArticles", new ValueProviderResult(tagArticles));
            return View("New");
        }

        private async Task AddArticle(Article article)
        {
            string fileName = GetArticleFileName(article.Title);
            string filePath = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", fileName);
            await System.IO.File.WriteAllTextAsync(filePath, article.MdContent);
            article.Path = fileName;
            article.TagArticles.Add(new TagArticle() { IdTag = article.SelectedTag.IdTag });
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
    }
}
