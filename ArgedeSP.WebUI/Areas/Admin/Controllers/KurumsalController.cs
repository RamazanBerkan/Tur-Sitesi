using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KurumsalController : Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private Dil SuankiDil;

        public KurumsalController(
            IAnahtarDegerBS anahtarDegerBS)
        {
            _anahtarDegerBS = anahtarDegerBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        //public IActionResult Kurumsal()
        //{
        //    OperationResult kurumsal_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.KurumsalSayfasi);
        //    ViewBag.Kurumsal = ((AnahtarDeger)kurumsal_OR.ReturnObject).Deger;

        //    return View();
        //}
    }
}