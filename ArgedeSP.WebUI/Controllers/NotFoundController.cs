using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult SayfaBulunamadi()
        {
          
            Response.StatusCode = 404;
            return View();
        }
    }
}
