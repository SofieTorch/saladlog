using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Controllers
{
    public class LikeArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
