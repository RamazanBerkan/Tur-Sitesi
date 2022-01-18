using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace MNS_FrontEnd.Controllers
{
    public class SearchController : Controller
    {
        private IUrunBS _urunBS;
        private Dil SuankiDil;

        public SearchController(IUrunBS urunBS)
        {
            _urunBS = urunBS;
            SuankiDil = CultureInfo.CurrentCulture.DilGetir();

        }
        public IActionResult Search(string searchTerm)
        {
            OperationResult urunler_OR = _urunBS.AktifUrunleriGetir(SuankiDil, "", searchTerm);
            List<Urun> urunler = (List<Urun>)urunler_OR.ReturnObject;
            return View(urunler);
        }
    }
}
