using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ArgedeSP.WebUI.Controllers
{
    public class HataController : Controller
    {
        public IActionResult Hata()
        {
            return View();
        }

        public IActionResult YetkisizGiris()
        {
            return View();
        }
    }
}