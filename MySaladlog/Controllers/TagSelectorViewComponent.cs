using Microsoft.AspNetCore.Mvc;
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

        public IViewComponentResult Invoke()
        {
            ViewBag.Tags = GetTags();
            Tag tag = new Tag();
            return View(tag);
        }

        private List<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }
    }
}
