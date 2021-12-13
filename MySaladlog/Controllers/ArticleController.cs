using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MySaladlog.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
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
                string updatedContent = obj.MdContent + $"  \n![{file.FileName}](../images/{newFileName})  \n";
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
            string fileName = GetArticleFileName(obj.Title);
            string filePath = Path.Combine(_webHost.ContentRootPath, "AppData", "Articles", fileName);
            await System.IO.File.WriteAllTextAsync(filePath, obj.MdContent);

            obj.IdUser = 1; // TODO: Change to current user Id
            obj.Path = fileName;

            _context.Articles.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
