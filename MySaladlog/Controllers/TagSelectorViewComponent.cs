using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySaladlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Controllers
{
    public class TagSelectorViewComponent : ViewComponent
    {
        private readonly SaladlogContext _context;
        public TagSelectorViewComponent(SaladlogContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(IEnumerable<TagArticle> articleTags)
        {
            ViewBag.ArticleTags = articleTags;

            List<SelectListItem> tagList = GetTags().ConvertAll(t => new SelectListItem()
            {
                Text = t.TagName,
                Value = t.IdTag.ToString(),
                Selected = false
            });

            ViewBag.tagList = tagList;

            Tag tag = new Tag();
            return View(tag);
        }

        private List<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }
    }
}
