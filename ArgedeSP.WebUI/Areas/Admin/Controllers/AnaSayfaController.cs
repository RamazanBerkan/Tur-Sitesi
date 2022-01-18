using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AnaSayfaController : Controller
    {
        public AnaSayfaController()
        {

        }
        public IActionResult AnaSayfa()
        {
            return View();
        }
    }
}