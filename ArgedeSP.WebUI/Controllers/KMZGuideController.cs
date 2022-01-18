using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNS_FrontEnd.Controllers
{
    public class KMZGuideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
