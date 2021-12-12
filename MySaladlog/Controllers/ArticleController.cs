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
            if(extension == ".jpeg" || extension == ".jpg" || extension == ".png")
            {
                string newFileName = Guid.NewGuid().ToString().Replace('-', '_') + "_" + file.FileName.Replace(' ', '_').Replace('-', '_');
                string saveImg = Path.Combine(_webHost.WebRootPath, "images", newFileName);
                FileStream stream = new FileStream(saveImg, FileMode.Create);
                await file.CopyToAsync(stream);

                string updatedContent = obj.MdContent + $"  \n![{file.FileName}](../images/{newFileName})  \n";
                ModelState.SetModelValue("MdContent", new ValueProviderResult(updatedContent, CultureInfo.InvariantCulture));
                ViewData["Message"] = null;
            }
            else
            {
                ViewData["Message"] = "Ups! Formato de imagen inválido";
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult PostArticle(Article obj)
        {
            _context.Articles.Add(obj);
            return RedirectToAction("Index");
        }
    }
}
