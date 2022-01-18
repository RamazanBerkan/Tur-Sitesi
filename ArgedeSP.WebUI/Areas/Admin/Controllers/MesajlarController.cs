using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ArgedeSP.Contracts.Models.DTO.Iletisim.Req;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    public class MesajlarController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index([FromForm]IletisimForm_REQ iletisimForm_REQ)
        {
            var mesaj = new MesajlarViewModel()
            {
                IletisimForm_REQ = iletisimForm_REQ
            };
            return RedirectToAction("Index");
        }
    }
}
