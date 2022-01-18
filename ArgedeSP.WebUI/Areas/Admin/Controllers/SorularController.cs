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
    public class SorularController : Controller
    {
        private IAnahtarDegerBS _anahtarDegerBS;
        private Dil SuankiDil;

        public SorularController(
            IAnahtarDegerBS anahtarDegerBS)
        {
            _anahtarDegerBS = anahtarDegerBS;

            SuankiDil = CultureInfo.CurrentCulture.DilGetir();
        }

        public IActionResult Sorular()
        {
            OperationResult sorular_OR = _anahtarDegerBS.AnahtarGetir(SuankiDil, Tanimlamalar.SorularSayfasi);
            ViewBag.Sorular = ((AnahtarDeger)sorular_OR.ReturnObject).Deger;

            return View();
        }
    }
}